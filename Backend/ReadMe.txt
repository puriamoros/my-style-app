This folder has to be copied or linked into a apache public folder.

Ensure that the following modules are enabled in <dir_apache>\conf\httpd.conf:
- LoadModule rewrite_module modules/mod_rewrite.so

If you get the following warning for every post or put: "Automatically populating $HTTP_RAW_POST_DATA is deprecated and will be removed in a future version"
- Ensure that the following line is NOT commented in your <dir_php>\php.ini: always_populate_raw_post_data = -1

Example (with symbolic link):
- Go to apache public folder.
- Create the link: mklink /D api.mystyleapp.com <dir_repo>\Backend

You can also insert a virtual host referencing this folder:
- Open <dir_apache>\conf\extra\httpd-vhosts.conf
- Add the virtual host:
	NameVirtualHost api.mystyleapp.com:80
	<VirtualHost api.mystyleapp.com:80>
		DocumentRoot "C:/xampp/htdocs/api.mystyleapp.com"
		ServerName api.mystyleapp.com
	</VirtualHost>
- Open C:\Windows\System32\drivers\etc\hosts
- Add the new host:
	127.0.0.1       api.mystyleapp.com