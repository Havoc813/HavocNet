<%@ Application Language="C#" %>
<%@ Import Namespace="Core" %>
<%@ Import Namespace="HavocNet" %>
<%@ Import Namespace="Phoenix.Core.Logging" %>
<%@ Import Namespace="Phoenix.Core.Servers" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e)
    {
        var aServer = new HavocServer();
        aServer.Open();
        
        var aError = new FSError(aServer);
        aError.LogInTable(HttpContext.Current.User.Identity.Name, "HavocNet", Request.Url.PathAndQuery, Server.GetLastError().GetBaseException());
        
        aServer.Close();
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
