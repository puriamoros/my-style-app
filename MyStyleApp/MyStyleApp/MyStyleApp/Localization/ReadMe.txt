Localization reference: https://developer.xamarin.com/guides/xamarin-forms/advanced/localization/

How to add a new language:
- Add a new resx resource named LocalizedStrings.<lang code>.resx to MyStyleApp\Localization.
- Add the new language code to MyStyleApp.iOS -> Info.plist (open it with a text editor).
- Add the new language code to MyStyleApp.WinPhone -> Package.appxmanifest (open it with a text editor).

How to use localization inside xaml files:
- To use it, just add a StaticResource in your App.xaml pointing to to this class. Example:
	<LocalizedStringsServiceNS:StringResourcesService x:Key="LocalizedStrings" />
- To bind some text to your ui component, use a binding. Example:
	Text="{Binding [put_the_string_key_inside_the_brackets], Source={StaticResource LocalizedStrings}}"
- It is possible to split long texts in smaller parts in the language files 
	(LocalizedStrings.resx). To do that, just split the text in some parts and name the
	keys in the following way: "long_text_key_0", ..., "long_text_key_n". To use
	this feature you must assure that no key named "long_text_key" exists.