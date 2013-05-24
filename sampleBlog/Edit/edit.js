var textLoaded = false;

function saveLink()
{
	saveText();
	var text = document.getElementById("wmd-input").value;
	var blob = new Blob([text], {type:'text/plain'});
	var link = document.getElementById("link");
	link.href = window.webkitURL.createObjectURL(blob);
	link.download = document.getElementById("filename").value;
}

function saveText()
{
	if(textLoaded == false)
		return;
	var text = document.getElementById("wmd-input").value;
	localStorage["edit content"] = text;
	console.log("Saved to localstorage");
}

function loadFile(evt)
{
	var file = evt.target.files[0];
	document.getElementById("filename").value = file.name;
	var fileReader = new FileReader();
	fileReader.onload = function(fle) 
	{
		var text = fle.target.result;
		document.getElementById("wmd-input").value = text;
		editor.refreshPreview();
		saveText();
	};
	fileReader.readAsText(file, "UTF-8");

	document.getElementById('file').value = null;
}

function saveFilename()
{
	localStorage["edit filename"] = document.getElementById('filename').value;
}

function removeMetaHeader(md)
{
	var lines = md.split("\n");
	for(var i = 0; i < lines.length; i++)
	{
		if(lines[i].trim() == "")
		{
			lines.splice(0, i + 1);
			break;
		}
	}
	return lines.join("\n");
}

var editor;

window.onload = function()
{
	document.getElementById('file').addEventListener('change', loadFile);
	document.getElementById('filename').value = localStorage["edit filename"];
	document.getElementById('filename').addEventListener('change', saveFilename);
	document.getElementById('link').addEventListener('mouseover', saveLink);

	var text = localStorage["edit content"];
	if(text == "" || text == undefined)
		text = "Date: \nAuthor: \nTitle: \n#LinkTitle: \n#LinkUrl: \n#Index: \n\n# Hello World\n\nthis is";
	document.getElementById('wmd-input').value = text;
	textLoaded = true;
	document.getElementById('wmd-input').addEventListener('input', saveText);
	document.getElementById('wmd-input').addEventListener('change', saveText);

	var converter = new Markdown.Converter();//getSanitizingConverter();
	converter.hooks.chain("preConversion", removeMetaHeader);
	editor = new Markdown.Editor(converter);
	editor.run();
}


