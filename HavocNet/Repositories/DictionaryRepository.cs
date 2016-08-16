using System.Collections.Generic;
using System.Globalization;
using Core;
using Phoenix.Core.Logging;
using Phoenix.Core.Servers;

namespace HavocNet.Repositories
{
    public class DictionaryRepository
    {
        private readonly HavocServer _havocServer;
        private readonly User _havocUser;
        private FSAudit _audit;
        private string _alert;

        public DictionaryRepository(HavocServer havocServer, User havocUser)
        {
            _havocServer = havocServer;
            _havocUser = havocUser;
        }

        public IEnumerable<WordDictionary> GetAll()
        {
            var aList = new List<WordDictionary>();

            _havocServer.Open();

            var aReader = _havocServer.ExecuteReader("SELECT * FROM dbo.tblDictionaries ORDER BY Ordering");

            while (aReader.Read())
            {
                var aWordDictionary = new WordDictionary
                    {
                        ID = int.Parse(aReader["DictionaryID"].ToString()),
                        Name = aReader["DictionaryName"].ToString(),
                        Description = aReader["DictionaryDescription"].ToString(),
                        Loaded = (aReader["Loaded"].ToString() == "1")
                    };
                aList.Add(aWordDictionary);
            }
            aReader.Close();

            _havocServer.Close();

            return aList;
        }

        public void Delete(int id)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);

            var aReader = _havocServer.ExecuteReader("SELECT * FROM dbo.tblDictionaries WHERE DictionaryID = @Param0");
            if (aReader.Read())
            {
                _havocServer.SQLParams.Add(aReader["DictionaryName"].ToString());
                _havocServer.SQLParams.Add(aReader["DictionaryDescription"].ToString());
            }
            aReader.Close();

            _havocServer.ExecuteNonQuery("DELETE FROM dbo.tblDictionaries WHERE DictionaryID = @Param0");
            _havocServer.ExecuteNonQuery("DELETE FROM dbo.tblWords WHERE DictionaryID = @Param0");

            _audit = new FSAudit(_havocServer, _havocUser.Username, "Dictionary Delete","DictionaryID=" + _havocServer.SQLParams[0] + ", Name=" + _havocServer.SQLParams[1] + ", Description=" + _havocServer.SQLParams[2]);
            _audit.Create();

            _havocServer.Close();
        }

        public void Save(string name, string description)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(name);
            _havocServer.SQLParams.Add(description);

            const string strSQL = "INSERT INTO dbo.tblDictionaries (DictionaryName, DictionaryDescription, Loaded) " +
                                  "SELECT " +
                                  "@Param0 AS DictionaryName, " +
                                  "@Param1 AS DictionaryDescription, " +
                                  "(SELECT MAX(Ordering) + 1 FROM dbo.tblDictionaries) AS Ordering, " +
                                  "0 AS Loaded ";

            _havocServer.ExecuteNonQuery(strSQL);

            _audit = new FSAudit(_havocServer, _havocUser.Username, "Dictionary Save", "Name=" + _havocServer.SQLParams[0] + ", Description=" + _havocServer.SQLParams[1]);
            _audit.Create();

            _havocServer.Close();
        }

        public void Update(int id, string name, string description)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);
            _havocServer.SQLParams.Add(name);
            _havocServer.SQLParams.Add(description);

            const string strSQL = "UPDATE dbo.tblDictionaries " +
                                  "SET DictionaryName = @Param1, " +
                                  "DictionaryDescription = @Param2 " +
                                  "WHERE " +
                                  "DictionaryID = @Param0";

            _havocServer.ExecuteNonQuery(strSQL);

            _audit = new FSAudit(_havocServer, _havocUser.Username, "Dictionary Update", "DictionaryID=" + _havocServer.SQLParams[0] + ", New Name=" + _havocServer.SQLParams[1] + ", New Description=" + _havocServer.SQLParams[2]);
            _audit.Create();

            _havocServer.Close();
        }

        public void MoveDown(int id)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);
            _havocServer.ExecuteNonQuery(RepositoryHelp.MakeMoveDownSQL("tblDictionaries", "DictionaryID"));

            _havocServer.Close();
        }

        public void MoveUp(int id)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);
            _havocServer.ExecuteNonQuery(RepositoryHelp.MakeMoveUpSQL("tblDictionaries","DictionaryID"));

            _havocServer.Close();
        }

        public void LoadDictionary(int id, string filename)
        {
            this.ClearWords(id);

            var ScrabbleScores = new Dictionary<string, int>();
            
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);

            var aReader = _havocServer.ExecuteReader("SELECT * FROM dbo.tblScrabbleScores");
            while (aReader.Read())
            {
                ScrabbleScores.Add(aReader["Letter"].ToString(), int.Parse(aReader["Score"].ToString()));
            }
            aReader.Close();

            var aStreamReader = new System.IO.StreamReader(filename);

            while (!aStreamReader.EndOfStream)
            {
                var strSQL = "";
                var aCode = "";
                var readLine = aStreamReader.ReadLine();
                if (readLine != null)
                {
                    var aWord = readLine.Trim().ToUpper();

                    if (aWord.IndexOf(" ", System.StringComparison.Ordinal) > 0)
                    {
                        aWord = aWord.Substring(0, aWord.IndexOf(" ", System.StringComparison.Ordinal));
                    }

                    _havocServer.SQLParams.Clear();
                    _havocServer.SQLParams.Add(aWord);
                    _havocServer.SQLParams.Add(id);
                    _havocServer.SQLParams.Add(aWord.Length);

                    int[] arr = CreateCode(aWord);

                    var j = 0;

                    int i;
                    for (i = 0; i <= 25; i++)
                    {
                        _havocServer.SQLParams.Add(arr[i]);
                        strSQL += ", @Param" + (i + 3).ToString(CultureInfo.InvariantCulture) + " AS " + (char)(i + 65);

                        if (arr[i] != 0)
                        {
                            aCode += (char)(i + 65) + arr[i].ToString(CultureInfo.InvariantCulture);
                            j += ScrabbleScores[((char)(i + 65)).ToString(CultureInfo.InvariantCulture)];
                        }
                    }

                    _havocServer.SQLParams.Add(aCode);
                    _havocServer.SQLParams.Add(j);
                }

                _havocServer.ExecuteNonQuery("INSERT INTO dbo.tblWords(Word,DictionaryID,Length,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,Code,Score) SELECT @Param0 AS Word, @Param1 AS DictionaryID, @Param2 AS Length" + strSQL + ", @Param29 AS Code, @Param30 AS Score");
            }

            _havocServer.ExecuteNonQuery("UPDATE dbo.tblDictionaries SET Loaded = 1 WHERE DictionaryID = @Param1");

            aStreamReader.Close();

            _audit = new FSAudit(_havocServer, _havocUser.Username,"Dictionary Clear Words","DictionaryID=" + _havocServer.SQLParams[0] + ", Name=" + _havocServer.SQLParams[1] + ", Description=" + _havocServer.SQLParams[2]);
            _audit.Create();

            _havocServer.Close();
        }

        public void ClearWords(int id)
        {
            _havocServer.Open();

            _havocServer.SQLParams.Add(id);

            var aReader = _havocServer.ExecuteReader("SELECT * FROM dbo.tblDictionaries WHERE DictionaryID = @Param0");
            if (aReader.Read())
            {
                _havocServer.SQLParams.Add(aReader["DictionaryName"].ToString());
                _havocServer.SQLParams.Add(aReader["DictionaryDescription"].ToString());
            }
            aReader.Close();

            _havocServer.ExecuteNonQuery("DELETE FROM dbo.tblWords WHERE DictionaryID = @Param0");

            _havocServer.ExecuteNonQuery("UPDATE dbo.tblDictionaries SET Loaded = 0 WHERE DictionaryID = @Param0");

            _audit = new FSAudit(_havocServer, _havocUser.Username,"Dictionary Clear Words","DictionaryID=" + _havocServer.SQLParams[0] + ", Name=" + _havocServer.SQLParams[1] + ", Description=" + _havocServer.SQLParams[2]);
            _audit.Create();

            _havocServer.Close();
        }

        private int[] CreateCode(string aWord)
        {
            var aArr = new int[26];

            foreach (var aChr in aWord.ToCharArray())
            {
                int j = aChr;

                if (j > 64 & j < 91)
                {
                    aArr[j - 65] += 1;
                }
                else
                {
                    _alert = aChr.ToString(CultureInfo.InvariantCulture);
                }
            }

            return aArr;
        }
    }

    public class WordDictionary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Loaded { get; set; }

        public string IsLoaded
        {
            get { return Loaded ? "True" : "False"; }
        }
    }
}
