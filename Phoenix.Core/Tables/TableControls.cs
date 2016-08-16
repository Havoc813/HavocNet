using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Phoenix.Core.Tables
{
    public class TableControls
    {
        private ImageButton _imgAdd;
        private ImageButton _imgEdit;
        private ImageButton _imgSave;
        private ImageButton _imgUpdate;
        private ImageButton _imgDelete;
        private ImageButton _imgCancel;
        private ImageButton _imgExport;
        private ImageButton _imgMoveUp;
        private ImageButton _imgMoveDown;

        private HiddenField _hidEditing;
        private HiddenField _hidSelected;

        public readonly int SelectedItemID;
        public readonly string SelectedRowID;

        public TableControls(string selectedRowID)
        {
            int.TryParse(selectedRowID.Replace("ctl00_cphMain_", ""), out SelectedItemID);
            this.SelectedRowID = selectedRowID;
        }

        public void BindControls(
            PlaceHolder controlLocation,
            Dictionary<string, ImageClickEventHandler> imageClickEventHandlers
            )
        {
            controlLocation.Controls.Clear();

            if (imageClickEventHandlers.ContainsKey("Add"))
            {
                _imgAdd = new ImageButton
                {
                    ID = "cmdAdd",
                    ImageUrl = @"~\App_Themes\Default\Images\NewImage.gif",
                    ToolTip = @"Add New",
                    CssClass = "TaskButtonPad",
                };
                _imgAdd.Click += imageClickEventHandlers["Add"];
                controlLocation.Controls.Add(_imgAdd);
            }

            if (imageClickEventHandlers.ContainsKey("Edit"))
            {
                _imgEdit = new ImageButton
                {
                    ID = "cmdEdit",
                    ImageUrl = @"~\App_Themes\Default\Images\EditImage.gif",
                    ToolTip = @"Edit",
                    CssClass = "TaskButtonPad",
                    OnClientClick = "javascript:return aTableManage.OnBeforeClick('edit');"
                };
                _imgEdit.Click += imageClickEventHandlers["Edit"];
                controlLocation.Controls.Add(_imgEdit);
            }

            if (imageClickEventHandlers.ContainsKey("Save"))
            {
                _imgSave = new ImageButton
                {
                    ID = "cmdSave",
                    ImageUrl = @"~\App_Themes\Default\Images\SaveImage.gif",
                    ToolTip = @"Save",
                    CssClass = "TaskButtonPad",
                    OnClientClick = "javascript:return aTableManage.OnBeforeSave();"
                };
                _imgSave.Click += imageClickEventHandlers["Save"];
                controlLocation.Controls.Add(_imgSave);
            }

            if (imageClickEventHandlers.ContainsKey("Update"))
            {
                _imgUpdate = new ImageButton
                {
                    ID = "cmdUpdate",
                    ImageUrl = @"~\App_Themes\Default\Images\UpdateImage.gif",
                    ToolTip = @"Update",
                    CssClass = "TaskButtonPad",
                    OnClientClick = "javascript:return aTableManage.OnBeforeUpdate();"
                };
                _imgUpdate.Click += imageClickEventHandlers["Update"];
                controlLocation.Controls.Add(_imgUpdate);
            }

            if (imageClickEventHandlers.ContainsKey("Delete"))
            {
                _imgDelete = new ImageButton
                {
                    ID = "cmdDelete",
                    ImageUrl = @"~\App_Themes\Default\Images\DeleteImage.gif",
                    ToolTip = @"Delete",
                    CssClass = "TaskButtonPad",
                    OnClientClick = "javascript:return aTableManage.OnBeforeDelete();"
                };
                _imgDelete.Click += imageClickEventHandlers["Delete"];
                controlLocation.Controls.Add(_imgDelete);
            }

            if (imageClickEventHandlers.ContainsKey("Cancel"))
            {
                _imgCancel = new ImageButton
                {
                    ID = "cmdCancel",
                    ImageUrl = @"~\App_Themes\Default\Images\CancelImage.gif",
                    ToolTip = @"Cancel",
                    CssClass = "TaskButtonPad"
                };
                _imgCancel.Click += imageClickEventHandlers["Cancel"];
                controlLocation.Controls.Add(_imgCancel);
            }

            if (imageClickEventHandlers.ContainsKey("Move Up"))
            {
                _imgMoveUp = new ImageButton
                {
                    ID = "cmdMoveUp",
                    ImageUrl = @"~\App_Themes\Default\Images\UpArrow.gif",
                    ToolTip = @"Move Up",
                    CssClass = "TaskButtonPad"
                };
                _imgMoveUp.Click += imageClickEventHandlers["Move Up"];
                controlLocation.Controls.Add(_imgMoveUp);
            }

            if (imageClickEventHandlers.ContainsKey("Move Down"))
            {
                _imgMoveDown = new ImageButton
                {
                    ID = "cmdMoveDown",
                    ImageUrl = @"~\App_Themes\Default\Images\DownArrow.gif",
                    ToolTip = @"Move Down",
                    CssClass = "TaskButtonPad"
                };
                _imgMoveDown.Click += imageClickEventHandlers["Move Down"];
                if (imageClickEventHandlers.ContainsKey("Move Down")) controlLocation.Controls.Add(_imgMoveDown);
            }

            if (imageClickEventHandlers.ContainsKey("Export"))
            {
                _imgExport = new ImageButton
                {
                    ID = "cmdExport",
                    ImageUrl = @"~\App_Themes\Default\Images\excel.gif",
                    ToolTip = @"Export To Excel",
                    CssClass = "TaskButtonPad"
                };
                _imgExport.Click += imageClickEventHandlers["Export"];
                controlLocation.Controls.Add(_imgExport);
            }

            _hidEditing = new HiddenField { ID = "hidEditingRow", Value = "" };

            _hidSelected = new HiddenField { ID = "hidSelectedRow", Value = "" };

            controlLocation.Controls.Add(_hidEditing);
            controlLocation.Controls.Add(_hidSelected);
        }

        public void BindVisibilities(bool pageAccess, string pageAction)
        {
            this.ClearRows();

            if (_imgAdd != null) _imgAdd.Visible = pageAction != "Add" && pageAccess;
            if (_imgEdit != null) _imgEdit.Visible = pageAction != "Edit" && pageAccess;
            if (_imgSave != null) _imgSave.Visible = pageAction == "Add" && pageAccess;
            if (_imgUpdate != null) _imgUpdate.Visible = pageAction == "Edit" && pageAccess;
            if (_imgDelete != null) _imgDelete.Visible = pageAction != "Add" && pageAction != "Edit" && pageAccess;
            if (_imgCancel != null) _imgCancel.Visible = (pageAction == "Add" || pageAction == "Edit") && pageAccess;
            if (_imgMoveUp != null) _imgMoveUp.Visible = (pageAction != "Add" && pageAction != "Edit") && pageAccess;
            if (_imgMoveDown != null) _imgMoveDown.Visible = (pageAction != "Add" && pageAction != "Edit") && pageAccess;
        }

        public void SetEditingRow(string rowID)
        {
            this._hidEditing.Value = rowID;
        }

        public void SetSelectedRow(string rowID)
        {
            this._hidSelected.Value = rowID;
        }

        public enum Direction
        {
            Up = 1,
            Down = 2
        }

        public static string MakeMoveSQL(Direction direction, string tableName, string objectIDName, string groupName = "", string filter = "")
        {
            var strSQL = "";
            var grp = (groupName != "");
            var filt = (filter != "");

            if (grp) strSQL += @"DECLARE @Group VARCHAR(20) SET @Group = (SELECT " + groupName + @" FROM dbo." + tableName + @" WHERE " + objectIDName + @" = @Param0)";

            strSQL += @"DECLARE @OldOrder VARCHAR(20) 
                DECLARE @NewOrder VARCHAR(20) 
                SET @OldOrder = (SELECT Ordering FROM dbo." + tableName + @" WHERE " + objectIDName + @" = @Param0) 
                SET @NewOrder = (SELECT TOP 1 Ordering FROM dbo." + tableName + @" WHERE Ordering " + (direction == Direction.Up ? "<" : ">") + @" @OldOrder 
                " + (grp ? " AND " + groupName + @" = @Group" : "") + @" 
                " + (filt ? " AND " + filter : "") + @" 
                ORDER BY Ordering " + (direction == Direction.Up ? "DESC" : "") + @") 
                IF (@NewOrder <> '') BEGIN 
                UPDATE dbo." + tableName + @" SET Ordering = @OldOrder WHERE " + objectIDName + @" = (SELECT " + objectIDName + @" FROM dbo." + tableName + @" WHERE Ordering = @NewOrder 
                " + (grp ? " AND " + groupName + @" = @Group" : "") + @" 
                " + (filt ? " AND " + filter : "") + @" ) 
                UPDATE dbo." + tableName + @" SET Ordering = @NewOrder WHERE " + objectIDName + @" = @Param0 
                END ";

            return strSQL;
        }

        public void ClearRows()
        {
            this._hidEditing.Value = "";
            this._hidSelected.Value = "";
        }
    }
}
