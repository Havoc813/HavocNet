function ob_OnNodeDrop(src, dst, copy)
{
	if(ob_ev("OnNodeDrop"))
	{
		if(document.getElementById(dst).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			dst = "root";
		} 
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("src", src);
	        window.ob_post.AddParam("dst", dst);

	        window.ob_post.post(null, "OnNodeDrop");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
	
	//Code to sort out the changing images
	var node = document.getElementById(src); 
	updateNodeLinkingLines(node, ob_getParentOfNode(node));
}

function ob_OnNodeDropOutside(dst)
{    
    ob_t2_CopyToControl(dst); 
    
    if(ob_ev("OnNodeDropOutside"))
	{
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("dst", dst);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnNodeDropOutside");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
}  

function ob_OnNodeSelect(id)
{
    //Load the selected member
    if (id.toString().substring(0,4) == 'Func')
    {
        SelectFunction(id);
    }
    else
    {
        if (id != "root_tree1") 
        {
            try
            {
                var dim = parent.selectedSubItem;
                var app = parent.selectedItem;
            
                if (dim != null && app != null)
                {
                    selectMember(id,dim,app);
                }
                else
                {
                    selectMember(id);
                }
            }
            catch(e){}
        }
        else
        {
            try
            {
                deselectMember();
            }
            catch(e){}
        }
    }
    
    if(ob_ev("OnNodeSelect"))
    {
        if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
	        id = "root";
        } 
        if(typeof window.ob_post == "object")
        {
            window.ob_post.AddParam("id", id);
            //Change "TreeEvents.aspx" with the name of your server-side processing file
            window.ob_post.post("process_edit.aspx", "OnNodeSelect");
        }
        else
        {
            alert("Please add obout_AJAXPage control to your page to use the server-side events");
        } 
    }
}

function ob_OnNodeEdit(id, text, prevText)
{    
    // add client side code here
	//alert("OnNodeEdit on " + id + "\n  text: " + text + "\n prevText: " + prevText);
		
	if(ob_ev("OnNodeEdit"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);
	        window.ob_post.AddParam("text", text);
	        window.ob_post.AddParam("prevText", prevText);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnNodeEdit");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	} 
}

function ob_OnAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL)
{
    document.getElementById('hidNewMemberID').value = Number(childId) + 1;
            
	if(ob_ev("OnAddNode"))
	{
		if(document.getElementById(parentId).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			parentId = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("parentId", parentId);
	        window.ob_post.AddParam("childId", childId);
	        window.ob_post.AddParam("textOrHTML", textOrHTML);
	        window.ob_post.AddParam("expanded", expanded ? expanded : 0);
	        window.ob_post.AddParam("image", image ? image : "");
	        window.ob_post.AddParam("subTreeURL", subTreeURL ? subTreeURL : "");
	        
	        window.ob_post.post(null, "OnAddNode");
	    } 		
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
}

function ob_OnRemoveNode(id)
{    
	 if(ob_ev("OnRemoveNode"))
	 {		
	    if(typeof window.ob_post == "object")
	    {			
	        window.ob_post.AddParam("ctrlID", id);
	        
	        window.ob_post.post(null, "OnRemoveNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	 }
}

function ob_OnNodeExpand(id, dynamic)
{
	try
	{
	    parent.stretchFrame('PageMain');
	}	     
	catch(e)
	{
	    try
	    {
	        parent.stretchSubFrame('mbrFrame'); 
	        parent.parent.stretchFrame('PageMain');
	    }
	    catch(e)
	    {
	        try
	        {
	            parent.stretchFrame('frameDetail'); 
	            parent.parent.stretchFrame('PageMain');
	        }
	        catch(e)
	        {}
	    }
	}
	window.status = '';
    
    if(ob_ev("OnNodeExpand"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnNodeExpand");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnNodeCollapse(id)
{
	try
	{
	    parent.stretchFrame('PageMain');
	}	     
	catch(e)
	{
	    try
	    {
	        parent.stretchSubFrame('mbrFrame'); 
	        parent.parent.stretchFrame('PageMain');
	    }
	    catch(e)
	    {
	        try
	        {
	            parent.stretchFrame('frameDetail'); 
	            parent.parent.stretchFrame('PageMain');
	        }
	        catch(e)
	        {}
	    }
	}
    if(ob_ev("OnNodeCollapse"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnNodeCollapse");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnMoveNodeUp(node_up_id, node_down_id)
{
    if(ob_ev("OnMoveNodeUp"))
	{		
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("nodeUpID", node_up_id);
	        window.ob_post.AddParam("nodeDownID", node_down_id);

	        window.ob_post.post(null, "OnNodeMoveUp");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnMoveNodeDown(node_down_id, node_up_id)
{
    if(ob_ev("OnMoveNodeDown"))
	{		
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("nodeDownID", node_down_id);
	        window.ob_post.AddParam("nodeUpID", node_up_id);

	        window.ob_post.post(null, "OnMoveNodeDown");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

/*
	Pre-events.
	Use them to implement your own validation for such operations as add, remove, edit
*/

function ob_OnBeforeAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL)
{       

	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeAddNode");			
	if(ob_ev("OnBeforeAddNode"))
	{
		if(document.getElementById(parentId).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			parentId = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("parentId", parentId);
	        window.ob_post.AddParam("childId", childId);
	        window.ob_post.AddParam("textOrHTML", textOrHTML);
	        window.ob_post.AddParam("expanded", expanded ? expanded : 0);
	        window.ob_post.AddParam("image", image ? image : "");
	        window.ob_post.AddParam("subTreeURL", subTreeURL ? subTreeURL : "");
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeAddNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	} 	
	return true;
}

function ob_OnBeforeRemoveNode(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeRemoveNode");	
	if(ob_ev("OnBeforeRemoveNode"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeRemoveNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeEdit(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeEdit");
	if(ob_ev("OnBeforeNodeEdit"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeEdit");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeSelect(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeSelect");
	
	if(ob_ev("OnBeforeNodeSelect"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeSelect");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeDrop(src, dst, copy)
{    
    try
    {
        MyOnNodeDrop(src,dst);
    }
    catch(e)
    {}
 
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid	
	//alert("Node with id:" + src + " will be " + (!copy ? "moved" : "copied") + " to node with id:" + dst);	
	if(ob_ev("OnBeforeNodeDrop"))
	{
		if(document.getElementById(dst).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			dst = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("src", src);
	        window.ob_post.AddParam("dst", dst);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeDrop");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	return true;
}

function ob_OnBeforeNodeDrag(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeDrag for node with id: " + id);
	
	if(ob_ev("OnBeforeNodeDrag"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeDrag");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	return true;
}


function ob_OnBeforeNodeDropOutside(dst)
{    
    // add client side code here
    //alert(dst.id)
    
    //alert("ob_OnBeforeNodeDropOutside");    
    if(ob_ev("OnBeforeNodeDropOutside"))
	{
	    if(typeof window.ob_post == "object")
	    {
	        //window.ob_post.AddParam("dst", dst);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        //window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeDropOutside");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
	
	return true;
} 

function ob_OnBeforeNodeExpand(id, dynamic)
{
    // add client side code here
	//alert("OnBeforeNodeExpand on: " + id + " " + dynamic);
    if(ob_ev("OnBeforeNodeExpand"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeExpand");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	
	return true;	
}

function ob_OnBeforeNodeCollapse(id)
{
    // add client side code here
	//alert("OnBeforeNodeCollapse on " + id);			
		
    if(ob_ev("OnBeforeNodeCollapse"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof window.ob_post == "object")
	    {
	        window.ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        window.ob_post.post("TreeEvents.aspx", "OnBeforeNodeCollapse");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	
	return true;
}
