This folder has to be copied or linked into a apache public folder.
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