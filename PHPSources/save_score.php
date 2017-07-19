<?php
	// 데이터베이스 접속
	$connect = mysql_connect("localhost", "유저아이디", "비밀번호");
	
	// 데이터베이스 접속 성공 여부 확인
	if($connect == 0){
		?>SORRY SCORE SERVER CONNECTION ERROR<?
	}else{}
	
	// 사용할 데이터베이스 설정
	mysql_select_db("데이터베이스명", $connect);
	
	// POST방식으로 전달된 파라미터를 변수에 저장
	$user_name = $_POST["user_name"];
	$kill_count = $_POST["kill_count"];
	$seq_no = $_POST["seq_no"];
	
	// SQL Query 생성
	$sql = "INSERT INTO tb_score (user_name, kill_count, seq_no)";
	$sql .= "\n VALUES ('".$user_name."',".$kill_count.",".$seq_no.")";
	$sql .= "\n ON DUPLICATE KEY UPDATE kill_count = kill_count + 1";
	
	// SQL Query 실행
	$result = mysql_query($sql, $connect);
	
	// Query 실행 결과 반환
	if($result){
		echo("YOUR SCORE SAVED.");
	} else {
		echo("error");
	}
	
	// 데이터베이스 접속 종료
	mysql_close($connect);
?>