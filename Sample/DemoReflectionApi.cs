//
// Sample showing the core Element-based API to create a dialog
//
using System;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Sample
{
	// Use the preserve attribute to inform the linker that even if I do not
	// use the fields, to not try to optimize them away.
	
	[Preserve (AllMembers=true)]
	class Settings {
		
	[Section]
		[Localize]
		public bool AccountEnabled;
		[Skip] public bool Hidden;
				
	[Section ("Account", "Your credentials")]
		
		[Entry ("Enter your login name")]
		[Localize]
		public string Login;
		
		[Password ("Enter your password")]
		[Localize]
		public string Password;
		
	[Section ("Autocapitalize, autocorrect and clear button")]
		
		[Localize][Entry (Placeholder = "Enter your name", AutocorrectionType = UITextAutocorrectionType.Yes, AutocapitalizationType = UITextAutocapitalizationType.Words, ClearButtonMode = UITextFieldViewMode.WhileEditing)]
		public string Name;
		
	[Section ("Time Editing")]
		[Localize]
		public TimeSettings TimeSamples;
		
	[Section ("Enumerations")]
		
		[Caption ("Favorite CLR type")]
		[Localize]
		public TypeCode FavoriteType;
		
	[Section ("Checkboxes")]
		[Checkbox]
		[Localize]
		bool English = true;
		
		[Checkbox]
		[Localize]
		bool Spanish;
		
	[Section ("Image Selection")]
		[Localize]
		public UIImage Top;
		public UIImage Middle;
		public UIImage Bottom;
		
	[Section ("Multiline")]
		[Caption ("This is a\nmultiline string\nall you need is the\n[Multiline] attribute")]
		[Multiline]
		[Localize]
		public string multi;
		
	[Section ("IEnumerable")]
		[RadioSelection ("ListOfString")] 
		[Localize]
		public int selected = 1;
		[Localize]
		public IList<string> ListOfString;
	}

	public class TimeSettings {
		[Localize]
		public DateTime Appointment;
		
		[Localize]
		[Date]
		public DateTime Birthday;
		
		[Localize]
		[Time]
		public DateTime Alarm;
	}
	
	public partial class AppDelegate 
	{
		Settings settings;
		
		public void DemoReflectionApi ()
		{	
			if (settings == null) {
				var image = UIImage.FromFile ("monodevelop-32.png");
				
				settings = new Settings () {
					AccountEnabled = true,
					Login = "postmater@localhost.com",
					TimeSamples = new TimeSettings () {
						Appointment = DateTime.Now,
						Birthday = new DateTime (1980, 6, 24),
						Alarm = new DateTime (2000, 1, 1, 7, 30, 0, 0)
					},
					FavoriteType = TypeCode.Int32,
					Top = image,
					Middle = image,
					Bottom = image,
					ListOfString = new List<string> () { 
						NSBundle.MainBundle.LocalizedString("One",""), 
						NSBundle.MainBundle.LocalizedString("Two",""), 
						NSBundle.MainBundle.LocalizedString("Three","") }
				};
			}
			
			var title = NSBundle.MainBundle.LocalizedString ("Settings", "");
			var bc = new BindingContext (null, settings, title);
			
			var dv = new DialogViewController (bc.Root, true);
			
			// When the view goes out of screen, we fetch the data.
			dv.ViewDisappearing += delegate {
				// This reflects the data back to the object instance
				bc.Fetch ();
				
				// Manly way of dumping the data.
				Console.WriteLine ("Current status:");
				Console.WriteLine (
				    "AccountEnabled:  {0}\n" +
				    "Login:           {1}\n" +
				    "Password:        {2}\n" +
					"Name:      	  {3}\n" +
				    "Appointment:     {4}\n" +
				    "Birthday:        {5}\n" +
				    "Alarm:           {6}\n" +
				    "Favorite Type:   {7}\n" + 
				    "IEnumerable idx: {8}", 
				    settings.AccountEnabled, settings.Login, settings.Password, settings.Name,
				    settings.TimeSamples.Appointment, settings.TimeSamples.Birthday, 
				    settings.TimeSamples.Alarm, settings.FavoriteType,
				    settings.selected);
			};
			navigation.PushViewController (dv, true);	
		}
	}
}