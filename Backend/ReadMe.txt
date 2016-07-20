---------------------------
HOW TO CONFIGURE THE BE
---------------------------

Basic Apache and PHP config:
----------------------------

Ensure that the following modules are enabled in <dir_apache>\conf\httpd.conf:
- LoadModule rewrite_module modules/mod_rewrite.so

This folder (<DIR_REPO>\Backend) has to be copied or linked into the apache public folder.

Example (with symbolic link):
- Go to apache public folder.
- Create the link: mklink /D api.mystyleapp.com <DIR_REPO>\Backend

You can also insert a virtual host referencing this folder:
- Open <dir_apache>\conf\extra\httpd-vhosts.conf
- Add the virtual host:
	NameVirtualHost api.mystyleapp.com:80
	<VirtualHost api.mystyleapp.com:80>
		DocumentRoot "<APACHE_PUBLIC_FOLDER>/api.mystyleapp.com"
		ServerName api.mystyleapp.com
	</VirtualHost>
- Open C:\Windows\System32\drivers\etc\hosts
- Add the new host:
	127.0.0.1       api.mystyleapp.com

To check if everything is ok:
- Open in a browser: http://api.mystyleapp.com/
- You should see: It's running!!


Common erros
--------------

If you get the following warning for every post or put: "Automatically populating $HTTP_RAW_POST_DATA is deprecated and will be removed in a future version"
- Ensure that the following line is NOT commented in your <dir_php>\php.ini: always_populate_raw_post_data = -1

If you get the following warning some calls: "Creating default object from empty value in <file_path>":
- Ensure that the following line is set to E_ERROR in your <dir_php>\php.ini: error_reporting = E_ERROR
