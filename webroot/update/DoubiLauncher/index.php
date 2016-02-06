<?php
	//服务器设置
	$ServerVersion = 0.1; //服务器处理程序版本
	$LastestVersion_Release = '0.1.0.0'; //最后的Release版本
	$LastestVersion_Beta = '0.1.0.0'; //最后的Beta版本
	$LastestVersion_Debug = '0.1.0.0'; //最后的Debug版本
	
	
	//处理请求
	switch (@$_GET['Command']){
		case 'GetServerVersion':
			echo $ServerVersion;
			break;
		case 'GetLastestVersion':
			switch (@$_GET['Version']){
				case 'Release':
					echo $LastestVersion_Release;
					break;
				case 'Beta':
					echo $LastestVersion_Beta;
					break;
				case 'Debug':
					echo $LastestVersion_Debug;
					break;
				default:
					echo 'Unknown Parameter';
			}
			break;
		case 'Download':
			//打开文件
			$file_path = "./Download/" . @$_GET['Version'] . "/" . @$_GET['Version'] . ".exe";
			$file_size = filesize ( "./Download/" . @$_GET['Version'] . "/" . @$_GET['Version'] . ".exe" );
			$file = fopen ( $filepath , "r" );
			//输入文件标签
			Header( "Content-type: application/octet-stream" );
			Header( "Accept-Ranges: bytes" );
			Header( "Accept-Length: " . $file_size );
			Header( "Content-Disposition: attachment; filename=" . @$_GET['Version'] . ".exe" );
			//输出文件内容
			//读取文件内容并直接输出到浏览器
			echo fread( $file, $file_size );
			fclose( $file );
			exit();
			break;
		default:
			echo 'Unknown Command';
	}

?>
