//// zoom control ////

// sets the zoom of the document
function setZoom(zoom) {
	document.body.style.zoom = zoom + "%";
}

//// search and highlight functions ////

// some variables
var spans = [];
var index = false;
var excluded = { script: true, style: true, iframe: true, canvas: true };

// mark matches and jump to the first find
function search(text) {
	// clear old marks
	removeSpans();
	// exit on empty string; no need to do something
	if (text == "") return;
	// insert new marks
	mark(document.body, new RegExp("(" + text.replace(/[.?*+^$[\]\\(){}|-]/g, "\\$&") + ")", "igm"));
	// collect marks
	spans = document.getElementsByTagName("span");
	// jump to first found mark and set it as current
	if (spans.length == 0) return;
	index = 0;
	setCurrent(0);
}

// recursively marks the matches of 'regex' in 'node' with an element 
// of type 'tagname'
function mark(node, regex, tagname) {
	var tagname = tagname || "span";
	var excluded = excluded || {};
	// get first child
	var child = node.firstChild;
	while (child) {
		if (child.nodeType == 1 && !excluded[child.nodeName]) {
			// recursive insert on non-excluded element nodes
			mark(child, regex);
		} else if (child.nodeType == 3) {
			// text nodes
			while ((match = regex.exec(child.nodeValue)) != null) {
				var s = child.nodeValue;
				// part before match
				node.insertBefore(document.createTextNode(s.substr(0, match.index)), child)
				// match
				var span = document.createElement(tagname);
				span.appendChild(document.createTextNode(match[0]));
				node.insertBefore(span, child)
				// part behind match; just adjust the text of the node
				child.nodeValue = s.substr(match.index + match[0].length);
			}
		}
		// next child please...
		child = child.nextSibling;
	}
}

// removes span elements from the document
// also clears some global variables
function removeSpans() {
	spans = [];
	index = false;
	document.body.innerHTML = document.body.innerHTML.replace(/(<\/?span[^>]*>)/igm, "");
}

// mark a finding as current and scroll it into view
// offset (int):
//    0 - current finding
//    1 - next finding
//   -1 - previous finding
function setCurrent(offset) {
	// no spans? Bye...
	if (index === false) return;
	// clear current
	spans[index].className = "";
	// set new index using modulu for the overflow
	index = (spans.length + index + offset) % spans.length;
	// set current
	spans[index].className = "current";
	// scroll in
	spans[index].scrollIntoView();
}