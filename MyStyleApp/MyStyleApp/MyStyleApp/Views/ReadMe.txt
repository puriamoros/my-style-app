﻿This is the base code for a View in the project:
------------------------------------------------


<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyStyleApp.Views.<PAGE_CLASS_NAME>"
             xmlns:controls="clr-namespace:MyStyleApp.Views.Controls"
             Padding="{StaticResource PagePadding}">
  <ScrollView>
    <Grid>
      <!-- Put the page contents here -->
      <controls:WaitingOverlayControl
        IsVisible="{Binding IsBusy}"
        Text="{Binding LocalizedStrings[waiting]}"/>
    </Grid>
  </ScrollView>
</ContentPage>