<?php
	// 데이터베이스 접속
	$connect = mysql_connect("localhost", "유저아이디", "비밀번호");
	if($connect == 0){
		?>SORRY SCORE SERVER CONNECTION ERROR<?
	} else {}
	
	// 사용할 데이터베이스 설정
	mysql_select_db("데이터베이스명", $connect);
	
	// POST방식으로 전달된 파라미터를 변수에 저장
	$seq_no = $_POST["seq_no"];
	
	// SQL Query 생성
	$sel  = "SELECT";
	$sel .= "\n @rownum:=@rownum+1 as ranking";
	$sel .= "\n 		, A.user_name as user_name";
	$sel .= "\n 		, A.kill_count as kill_count";
	$sel .= "\n FROM tb_score as A, (SELECT @rownum := 0) R ";
	$sel .= "\n WHERE A.seq_no = '" .$seq_no."'";
	$sel .= "\n ORDER BY kill_count DESC";
	
	// SQL Query 실행
	$result = mysql_query($sel, $connect);
	
	// JSON생성을 위한 배열
	$rows 	= array();
	$return = array();
	
	// Query 결과값을 처음부터 마지막 레코드까지 진행하면서 추출
	while($row = mysql_fetch_array($result))
	{
		$rows["ranking"]	= $row["ranking"];
		$rows["user_name"]	= $row["user_name"];
		$rows["kill_count"]	= $row["kill_count"];
		
		// JSON 데이터 생성을 위한 배열에 레코드 값 추가
		array_push($return, $rows);
	}
	
	header("Content-type:application/json");
	
	// JSON파일 생성
	echo json_encode($return);
	
	// DB 접속 종료
	mysql_close($connect);
?>