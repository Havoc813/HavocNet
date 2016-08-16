using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class FSTree
    {
        private readonly obout_ASPTreeView_2_NET.Tree _oTree = new obout_ASPTreeView_2_NET.Tree();
        private bool _hasItems;

        public FSTree()
        {
            _oTree.ShowIcons = false;
            _oTree.KeyNavigationEnable = true;

            _oTree.FolderScript = "../App_Themes/HavocNet/Images/tree2/script";
            _oTree.FolderStyle = "../App_Themes/HavocNet/Images/tree2/style2";

            this._hasItems = false;

            _oTree.EventList = "OnNodeDrop,OnAddNode,OnRemoveNode,OnMoveNodeUp,OnMoveNodeDown,OnBeforeNodeDropOutside";
        }

        public void AddNode(string parentID, string memberID, string memberName, string icon = "", string aSubTree = null, int numberOfChildren = 0)
        {
            this.SetWidth(347);
            this._hasItems = true;

            if (parentID == "0")
            {
                _oTree.AddRootNode("<b>" + memberName + " Dimension</b>", true, icon);
            }
            else if (parentID == "1")
            {
                _oTree.Add("root", Convert.ToString(memberID), memberName, false, null);
                if (numberOfChildren != 0)
                    AddNonDragable(Convert.ToString(memberID));
            }
            else
            {
                _oTree.Add(Convert.ToString(parentID), Convert.ToString(memberID), memberName, false, null, aSubTree);
                if (numberOfChildren != 0)
                    AddNonDragable(Convert.ToString(memberID));
            }
        }

        public void AddCheckNode(string parentID, string memberID, string memberName, string memberAlias, bool @checked, bool enabled = true)
        {
            this.SetWidth(500);
            this._hasItems = true;

            if (parentID == "1")
                parentID = "root";

            string chk = "";
            if (@checked)
                chk = "checked=\"true\"";

            string tst2 = "";
            if (!enabled)
                tst2 = "disabled=\"disabled\"";

            string tst = memberAlias;
            if (memberAlias.IndexOf("<", StringComparison.Ordinal) != -1)
                tst = memberAlias.Substring(0, memberAlias.IndexOf("<", StringComparison.Ordinal) - 1);
            _oTree.Add(parentID, memberID, "<input type=\"checkbox\" " + tst2 + " onclick=\"javascript:CheckChange(this,'" + tst + "')\" id=\"chk" + memberName + "\" " + chk + " /> " + memberAlias, false, null);
        }

        public void AddDropDownNode(string parentID, string memberID, string memberName, string memberAlias, string options, bool enabled = true)
        {
            this._hasItems = true;

            var nodeText = "<table><tr><td>" +
                            "<select id='cbo" + memberID + "' " + (enabled ? "" : " disabled='disabled'") + " " +
                            "onchange='javascript:DDLChange(this);'>";

            foreach (var aStr in options.Split('|'))
            {
                nodeText += "<option value='" + aStr.Split(';')[0] + "' " +
                            aStr.Split(';')[2] + ">" +
                            aStr.Split(';')[1] + "</option>";
            }

            nodeText += "</select></td>" +
                            "<td style='padding:3px'>" + memberName + "</td>" +
                            "<td><input type='hidden' id='hid" + memberID + "' value='" + memberAlias + "'></td>" +
                            "</tr></table>";

            _oTree.Add(parentID == "1" ? "root" : parentID, memberID, nodeText, false, null);
        }

        public void AddRootNode(string memberName)
        {
            _oTree.AddRootNode(memberName, true, "DimRoot.GIF");
        }

        public void SetWidth(int newWidth)
        {
            _oTree.Width = Convert.ToString(newWidth) + "px";
        }

        public void SetID(string newID)
        {
            _oTree.id = newID;
        }

        public bool DragAndDropEnable
        {
            set { _oTree.DragAndDropEnable = value; }
        }

        public string GetList
        {
            get
            {
                if (!this.HasItems)
                    this.AddNode("root", "Empty", "<span style='font-style:italic'>None</span>");

                return _oTree.HTML();
            }
        }

        public bool SubTree
        {
            set { _oTree.SubTree = value; }
        }

        public void AddNonDragable(string nodeID)
        {
            if (_oTree.DragDisableId.Length >= 1)
            {
                _oTree.DragDisableId += "," + nodeID;
            }
            else
            {
                _oTree.DragDisableId = nodeID;
            }
        }

        public void AddNonDropable(string nodeID)
        {
            if (_oTree.DropDisableId.Length >= 1)
            {
                _oTree.DropDisableId += "," + nodeID;
            }
            else
            {
                _oTree.DropDisableId = nodeID;
            }
        }

        public bool HasItems
        {
            get { return this._hasItems; }
        }
    }
}
