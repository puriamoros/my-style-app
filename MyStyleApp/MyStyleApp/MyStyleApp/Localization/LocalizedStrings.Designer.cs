﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyStyleApp.Localization {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class LocalizedStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocalizedStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyStyleApp.Localization.LocalizedStrings", typeof(LocalizedStrings).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add to favourites.
        /// </summary>
        internal static string add_to_favourites {
            get {
                return ResourceManager.GetString("add_to_favourites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Any old.
        /// </summary>
        internal static string any_old {
            get {
                return ResourceManager.GetString("any_old", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to cancel an appointment. Do you want to continue with the cancellation?.
        /// </summary>
        internal static string appointment_cancel_body {
            get {
                return ResourceManager.GetString("appointment_cancel_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel appointment.
        /// </summary>
        internal static string appointment_cancel_title {
            get {
                return ResourceManager.GetString("appointment_cancel_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The cancellation limit for an appointment es one day before the appointment date.
        /// </summary>
        internal static string appointment_cancellation_error {
            get {
                return ResourceManager.GetString("appointment_cancellation_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to confirm an appointment. Do you want to continue with the confirmation?.
        /// </summary>
        internal static string appointment_confirm_body {
            get {
                return ResourceManager.GetString("appointment_confirm_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm appointment.
        /// </summary>
        internal static string appointment_confirm_title {
            get {
                return ResourceManager.GetString("appointment_confirm_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pending confirmation.
        /// </summary>
        internal static string appointment_status_0 {
            get {
                return ResourceManager.GetString("appointment_status_0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirmed.
        /// </summary>
        internal static string appointment_status_1 {
            get {
                return ResourceManager.GetString("appointment_status_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancelled.
        /// </summary>
        internal static string appointment_status_2 {
            get {
                return ResourceManager.GetString("appointment_status_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Appointments.
        /// </summary>
        internal static string appointments {
            get {
                return ResourceManager.GetString("appointments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Book.
        /// </summary>
        internal static string book {
            get {
                return ResourceManager.GetString("book", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to request a booking with the following data:\n- Establishment: ${ESTABLISMENT_NAME}\n- Service: ${SERVICE_NAME}\n- Date: ${DATE}\n\nDo you want to continue with the request?.
        /// </summary>
        internal static string booking_data_body {
            get {
                return ResourceManager.GetString("booking_data_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Booking Reaquest.
        /// </summary>
        internal static string booking_data_title {
            get {
                return ResourceManager.GetString("booking_data_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Booking at the requested date and time is not possible. Please, select a different date and/or time.
        /// </summary>
        internal static string booking_error_body {
            get {
                return ResourceManager.GetString("booking_error_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not book.
        /// </summary>
        internal static string booking_error_title {
            get {
                return ResourceManager.GetString("booking_error_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The booking request has been saved. You can see it in the Appointments section. Remember that you will receive a confirmation email when your request is accepted.
        /// </summary>
        internal static string booking_requested_body {
            get {
                return ResourceManager.GetString("booking_requested_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Booking Requested.
        /// </summary>
        internal static string booking_requested_title {
            get {
                return ResourceManager.GetString("booking_requested_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string cancel {
            get {
                return ResourceManager.GetString("cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Change password.
        /// </summary>
        internal static string change_password {
            get {
                return ResourceManager.GetString("change_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose an item.
        /// </summary>
        internal static string choose_an_item {
            get {
                return ResourceManager.GetString("choose_an_item", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose the date.
        /// </summary>
        internal static string choose_date {
            get {
                return ResourceManager.GetString("choose_date", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose the hour.
        /// </summary>
        internal static string choose_hour {
            get {
                return ResourceManager.GetString("choose_hour", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm.
        /// </summary>
        internal static string confirm {
            get {
                return ResourceManager.GetString("confirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create an account.
        /// </summary>
        internal static string create_account {
            get {
                return ResourceManager.GetString("create_account", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Created account.
        /// </summary>
        internal static string created_account {
            get {
                return ResourceManager.GetString("created_account", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        internal static string date {
            get {
                return ResourceManager.GetString("date", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete favourite.
        /// </summary>
        internal static string delete_favourite {
            get {
                return ResourceManager.GetString("delete_favourite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login.
        /// </summary>
        internal static string do_login {
            get {
                return ResourceManager.GetString("do_login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        internal static string edit {
            get {
                return ResourceManager.GetString("edit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email.
        /// </summary>
        internal static string email {
            get {
                return ResourceManager.GetString("email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your email address.
        /// </summary>
        internal static string email_placeholder {
            get {
                return ResourceManager.GetString("email_placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string error {
            get {
                return ResourceManager.GetString("error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: The entered email is already registered.
        /// </summary>
        internal static string error_duplicated_email {
            get {
                return ResourceManager.GetString("error_duplicated_email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; must have ${FIELD_LENGTH} characters.
        /// </summary>
        internal static string error_field_length {
            get {
                return ResourceManager.GetString("error_field_length", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; can not have more than ${FIELD_LENGTH_MAX} characters.
        /// </summary>
        internal static string error_field_length_max {
            get {
                return ResourceManager.GetString("error_field_length_max", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; must have at least ${FIELD_LENGTH_MIN} characters.
        /// </summary>
        internal static string error_field_length_min {
            get {
                return ResourceManager.GetString("error_field_length_min", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; must have between ${FIELD_LENGTH_MIN} and ${FIELD_LENGTH_MAX} characters.
        /// </summary>
        internal static string error_field_length_range {
            get {
                return ResourceManager.GetString("error_field_length_range", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fields &quot;${FIELD_NAME_1}&quot; and &quot;${FIELD_NAME_2}&quot; must be diferent.
        /// </summary>
        internal static string error_fields_equal {
            get {
                return ResourceManager.GetString("error_fields_equal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fields &quot;${FIELD_NAME_1}&quot; and &quot;${FIELD_NAME_2}&quot; must be equal.
        /// </summary>
        internal static string error_fields_not_equal {
            get {
                return ResourceManager.GetString("error_fields_not_equal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; can not contain &quot;&lt;&quot; or &quot;&gt;&quot;.
        /// </summary>
        internal static string error_insecure_chars {
            get {
                return ResourceManager.GetString("error_insecure_chars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; is not valid.
        /// </summary>
        internal static string error_invalid_field {
            get {
                return ResourceManager.GetString("error_invalid_field", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &quot;${FIELD_NAME}&quot; is required.
        /// </summary>
        internal static string error_required_field {
            get {
                return ResourceManager.GetString("error_required_field", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Establishment.
        /// </summary>
        internal static string establishment {
            get {
                return ResourceManager.GetString("establishment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Details.
        /// </summary>
        internal static string establishment_details {
            get {
                return ResourceManager.GetString("establishment_details", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Establishment type.
        /// </summary>
        internal static string establishment_type {
            get {
                return ResourceManager.GetString("establishment_type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hairdresser.
        /// </summary>
        internal static string establishment_type_1 {
            get {
                return ResourceManager.GetString("establishment_type_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aesthetics.
        /// </summary>
        internal static string establishment_type_2 {
            get {
                return ResourceManager.GetString("establishment_type_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hairdresser and aesthetics.
        /// </summary>
        internal static string establishment_type_3 {
            get {
                return ResourceManager.GetString("establishment_type_3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Establishments.
        /// </summary>
        internal static string establishments {
            get {
                return ResourceManager.GetString("establishments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Favourites.
        /// </summary>
        internal static string favourites {
            get {
                return ResourceManager.GetString("favourites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There has been an error.\nPlease, check your internet connection or try again later..
        /// </summary>
        internal static string generic_error {
            get {
                return ResourceManager.GetString("generic_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to en.
        /// </summary>
        internal static string language_code {
            get {
                return ResourceManager.GetString("language_code", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Log out.
        /// </summary>
        internal static string log_out {
            get {
                return ResourceManager.GetString("log_out", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login.
        /// </summary>
        internal static string login {
            get {
                return ResourceManager.GetString("login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid credentials.
        /// </summary>
        internal static string login_error {
            get {
                return ResourceManager.GetString("login_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logout.
        /// </summary>
        internal static string logout {
            get {
                return ResourceManager.GetString("logout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to My account.
        /// </summary>
        internal static string my_account {
            get {
                return ResourceManager.GetString("my_account", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to My establishments.
        /// </summary>
        internal static string my_establishments {
            get {
                return ResourceManager.GetString("my_establishments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string name {
            get {
                return ResourceManager.GetString("name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your name.
        /// </summary>
        internal static string name_placeholder {
            get {
                return ResourceManager.GetString("name_placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New establishment.
        /// </summary>
        internal static string new_establishment {
            get {
                return ResourceManager.GetString("new_establishment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New password.
        /// </summary>
        internal static string new_password {
            get {
                return ResourceManager.GetString("new_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New staff.
        /// </summary>
        internal static string new_staff {
            get {
                return ResourceManager.GetString("new_staff", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No.
        /// </summary>
        internal static string no {
            get {
                return ResourceManager.GetString("no", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Don&apos;t have an account?.
        /// </summary>
        internal static string no_account {
            get {
                return ResourceManager.GetString("no_account", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The search returned no results. Please change the criteria and try again..
        /// </summary>
        internal static string no_search_results {
            get {
                return ResourceManager.GetString("no_search_results", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ok.
        /// </summary>
        internal static string ok {
            get {
                return ResourceManager.GetString("ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Old password.
        /// </summary>
        internal static string old_password {
            get {
                return ResourceManager.GetString("old_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        internal static string password {
            get {
                return ResourceManager.GetString("password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your password.
        /// </summary>
        internal static string password_placeholder {
            get {
                return ResourceManager.GetString("password_placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone.
        /// </summary>
        internal static string phone {
            get {
                return ResourceManager.GetString("phone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your telephone number.
        /// </summary>
        internal static string phone_placeholder {
            get {
                return ResourceManager.GetString("phone_placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please log in .
        /// </summary>
        internal static string please_login {
            get {
                return ResourceManager.GetString("please_login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Province.
        /// </summary>
        internal static string province {
            get {
                return ResourceManager.GetString("province", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remember me.
        /// </summary>
        internal static string remember_me {
            get {
                return ResourceManager.GetString("remember_me", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Repeat your email.
        /// </summary>
        internal static string repeat_email {
            get {
                return ResourceManager.GetString("repeat_email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Repeat new password.
        /// </summary>
        internal static string repeat_new_password {
            get {
                return ResourceManager.GetString("repeat_new_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Repeat your password.
        /// </summary>
        internal static string repeat_password {
            get {
                return ResourceManager.GetString("repeat_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Results.
        /// </summary>
        internal static string results {
            get {
                return ResourceManager.GetString("results", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save changes.
        /// </summary>
        internal static string save_changes {
            get {
                return ResourceManager.GetString("save_changes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search.
        /// </summary>
        internal static string search {
            get {
                return ResourceManager.GetString("search", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service.
        /// </summary>
        internal static string service {
            get {
                return ResourceManager.GetString("service", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service category.
        /// </summary>
        internal static string service_category {
            get {
                return ResourceManager.GetString("service_category", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Staff.
        /// </summary>
        internal static string staff {
            get {
                return ResourceManager.GetString("staff", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Surname.
        /// </summary>
        internal static string surname {
            get {
                return ResourceManager.GetString("surname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your surname.
        /// </summary>
        internal static string surname_placeholder {
            get {
                return ResourceManager.GetString("surname_placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User type.
        /// </summary>
        internal static string user_type {
            get {
                return ResourceManager.GetString("user_type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client.
        /// </summary>
        internal static string user_type_1 {
            get {
                return ResourceManager.GetString("user_type_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Owner.
        /// </summary>
        internal static string user_type_2 {
            get {
                return ResourceManager.GetString("user_type_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Staff.
        /// </summary>
        internal static string user_type_3 {
            get {
                return ResourceManager.GetString("user_type_3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authorized staff.
        /// </summary>
        internal static string user_type_4 {
            get {
                return ResourceManager.GetString("user_type_4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View details.
        /// </summary>
        internal static string view_details {
            get {
                return ResourceManager.GetString("view_details", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ...waiting....
        /// </summary>
        internal static string waiting {
            get {
                return ResourceManager.GetString("waiting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string warning {
            get {
                return ResourceManager.GetString("warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi ${USER_NAME}!.
        /// </summary>
        internal static string welcome_user {
            get {
                return ResourceManager.GetString("welcome_user", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        internal static string yes {
            get {
                return ResourceManager.GetString("yes", resourceCulture);
            }
        }
    }
}
