////////////////////////////////////////////////////////////////////////////////////////
//
//	Attaches a glowing look to text.
//
//	Requires: jquery 1.4.2 or better.
//
//	Usage:
//
//		Add a 1px glow around all text in all elements defined by a class name:
//			FAZScripts.addGlow(".glowtext", "glowForeground", "glowBackground", 1);
//
//		Add a 1px glow around all text in all div elements:
//			FAZScripts.addGlow("div", "glowForeground", "glowBackground", 1);
//
//		Add a 1px glow around all text in all elements with an ID of glow:
//			FAZScripts.addGlow("#glow", "glowForeground", "glowBackground", 1);	
//
//		You can use any valid jquery selector, see jquery documentation for more info.
//
//
//	Sample Styles:
//
//	<style type="text/css">
//		.glowBackground { color: Yellow; }
//		.glowForeground { color: Black; }
//		.glowtext { font-family: Verdana; }	
//	</style>
//
//	This comment may not be removed:
//
//	Part of the FreeAllegiance CSS System (http://www.freeallegiance.org).
//	
//	3/26/2010 - Nick Pirocanac (nick@chi-town.com)
///////////////////////////////////////////////////////////////////////////////////////

if (!FAZScripts)
{
	var FAZScripts = function()
	{

	}
}

if (!FAZScripts.addGlow)
{
	FAZScripts.addGlow = function(targetSelector, foregroundClass, backgroundClass, glowSizePX)
	{
		var glowElements = $(targetSelector);

		for (var i = 0; i < glowElements.length; i++)
		{
			var glowElement = glowElements[i];
			var currentHtml = glowElement.innerHTML;
			var glowElementWidth = $(glowElement).width() + (glowSizePX * 2);
			
			glowElement.style.width = glowElementWidth + "px";

			var divRelativeContainer = document.createElement("div");
			divRelativeContainer.style.position = "relative";
			divRelativeContainer.style.height = $(glowElement).height() + "px";
			divRelativeContainer.style.width = glowElementWidth + "px";

			while (glowElement.hasChildNodes() == true)
				glowElement.removeChild(glowElement.firstChild);

			for (var x = 0; x <= glowSizePX * 2; x++)
			{
				for (var y = 0; y <= glowSizePX * 2; y++)
				{
					$(divRelativeContainer).append('<div class="' + backgroundClass + '" style="position: absolute; top: ' + y + 'px; left: ' + x + 'px;">' + currentHtml + '</div>');
				}
			}

			$(divRelativeContainer).append('<div class="' + foregroundClass + '" style="position: absolute; top: ' + glowSizePX + 'px; left: ' + glowSizePX + 'px;">' + currentHtml + '</div>');

			$(glowElement).append(divRelativeContainer);
		}
	}
}