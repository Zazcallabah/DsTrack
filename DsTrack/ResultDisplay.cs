using System.Collections.Generic;
using System.Linq;

namespace DsTrack
{
	public class ResultDisplay
	{
		readonly Handler _handler;
		readonly TemplateFile _templates;

		public ResultDisplay( Handler handler, TemplateFile templates = null )
		{
			_handler = handler;
			_templates = templates;
		}

		public IEnumerable<string> Display()
		{
			var initialOffset = _handler.InitialOffset;
			var labels = _handler.CreateLabelStorage();
			foreach( var result in _handler.Make() )
			{
				var currentline = "";
				if( _templates != null )
					currentline = _templates.EntryFor( result, initialOffset );
				yield return string.Format( "{0:X5}-{1:0}: {2} ({3})", result.BitOffset() + initialOffset, result.Bit % 8, labels.Labels[result.LabelIndex], currentline );
			}
		}
	}
}