function performSorting(id) {
    var node = document.getElementById(id);
    
    var parent = ob_getParentOfNode(node);
    
    var tempNode = ob_getFirstChildOfNode(parent);
    
    if(tempNode == node) {
        tempNode = ob_getNextSiblingOfNode(node);
    }
    
    var nodeToReplace = null;
    
    while(tempNode != null) {
        if(tempNode.innerHTML.toString().toLowerCase() > node.innerHTML.toString().toLowerCase()) {
            nodeToReplace = tempNode;
            
            break;
        }
        
        tempNode = ob_getNextSiblingOfNode(tempNode);
    }
    
    if(tempNode != node) {        
        var parentContainer = parent.parentNode.parentNode.parentNode.nextSibling.firstChild.firstChild.childNodes[1];
        
        var nodeContainer = node.parentNode.parentNode.parentNode.parentNode;        
        
        nodeContainer = parentContainer.removeChild(nodeContainer);        
        
        if(nodeToReplace != null) {
            var nodeToReplaceContainer = nodeToReplace.parentNode.parentNode.parentNode.parentNode;
            parentContainer.insertBefore(nodeContainer, nodeToReplaceContainer);
        } else {
            parentContainer.appendChild(nodeContainer);
        }
    }                
    
    updateNodeLinkingLines(node, parent);
    
    if(typeof(ob_t51) == 'function') {
        ob_t51();
    }
}

function updateNodeLinkingLines(node, parent) {
    var isLast = (ob_getLastChildOfNode(parent) == node);

    var lastChild;
    var intermediateChild;
    
    if(!isLast) {
        intermediateChild = node;
        lastChild = ob_getLastChildOfNode(parent);
    } else {
        intermediateChild = ob_getPrevSiblingOfNode(node);
        lastChild = node;
    }
    
    applyLinkingLinesChangesToNode(intermediateChild, {
        'ImageOld': '_l.gif', 
        'ImageNew': '.gif', 
        'CheckNewImage': false, 
        'CssOld': 'ob_t6', 
        'CssNew': 'ob_t6v', 
        'ChildrenCssOld': '', 
        'ChildrenCssNew': 'ob_t5v',
        'ClearBgImageAttribute': true,
        'ClearChildBgImageAttribute': true            
    });
    applyLinkingLinesChangesToNode(lastChild, {
        'ImageOld': '.gif', 
        'ImageNew': '_l.gif', 
        'CheckNewImage': true, 
        'CssOld': 'ob_t6v', 
        'CssNew': 'ob_t6', 
        'ChildrenCssOld': 'ob_t5v', 
        'ChildrenCssNew': '',
        'ClearBgImageAttribute': true,
        'ClearChildBgImageAttribute': true
    });
}

function applyLinkingLinesChangesToNode(node, param) {
    if(node == null) {
        return;
    }
    var nodeLinkingLineImage = node.parentNode.firstChild.firstChild;
    var nodeLinkingLineImageName = nodeLinkingLineImage.src.toString();
    
    if(!param.CheckNewImage || nodeLinkingLineImageName.indexOf(param.ImageNew) == -1) {
        nodeLinkingLineImage.src = nodeLinkingLineImageName.replace(param.ImageOld, param.ImageNew);
    }
    
    if(nodeLinkingLineImage.parentNode.className.indexOf(param.CssOld) != -1) {
        nodeLinkingLineImage.parentNode.className = param.CssNew;
        if(param.ClearBgImageAttribute) {
            nodeLinkingLineImage.parentNode.style.backgroundImage = '';
        }
    }
    
    var childrenContainer = node.parentNode.parentNode.parentNode.nextSibling;
    if(childrenContainer && childrenContainer.firstChild) {
        childrenContainer.firstChild.firstChild.firstChild.className = param.ChildrenCssNew;
        if(param.ClearChildBgImageAttribute) {
            childrenContainer.firstChild.firstChild.firstChild.style.backgroundImage = '';
        }
    }
}