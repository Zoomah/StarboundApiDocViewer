function setZoom(zoom) {
	document.body.style.zoom = zoom + "%";
}

var spans = [];
var index = false;

function search(text) {
	removeSpans();

	if (text == "") return;
	insertSpans(document.body, new RegExp("(" + text.replace(/[.?*+^$[\]\\(){}|-]/g, "\\$&") + ")", "igm"));
	spans = document.getElementsByTagName("span");

	if (spans.length == 0) return;
	index = 0;
	setCurrent(0);
}

function insertSpans(node, regex) {
	var child = node.firstChild;
	while (child) {
		if (child.nodeType == 1) {
			insertSpans(child, regex);
		} else if (child.nodeType == 3) {
			while ((match = regex.exec(child.nodeValue)) != null) {
				var s = child.nodeValue;

				node.insertBefore(document.createTextNode(s.substr(0, match.index)), child)

				var span = document.createElement('span');
				span.appendChild(document.createTextNode(match[0]));
				node.insertBefore(span, child)

				child.nodeValue = s.substr(match.index + match[0].length);
			}
		}
		child = child.nextSibling;
	}
	return node;
}

function removeSpans() {
	spans = [];
	index = false;
	document.body.innerHTML = document.body.innerHTML.replace(/(<span[^>]*>|<\/span>)/igm, "");
}

function setCurrent(offset) {
	if (index === false) return;
	spans[index].className = "";
	index = (spans.length + index + offset) % spans.length;
	spans[index].className = "current";
	spans[index].scrollIntoView();
}