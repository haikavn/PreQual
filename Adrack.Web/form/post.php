<?php
    // header('Content-type: application/xml');
 if(isset($_REQUEST['get_country'])) {
     $country = file_get_contents('https://api.ipgeolocation.io/ipgeo?apiKey=04121b22f4244f55a04a496edcc8fd9a&include=hostname&ip=' . $_SERVER['REMOTE_HOST']);
     echo json_decode($country)->country_code2;
     exit;
 }

$xmpStr = '
   <REQUEST>
    <REFERRAL>
        <CHANNELID>31b6474</CHANNELID>
        <PASSWORD>a0555f21</PASSWORD>
        <AFFSUBID></AFFSUBID>
        <AFFSUBID2></AFFSUBID2>
        <AFFSUBID3></AFFSUBID3>
        <AFFSUBID4></AFFSUBID4>
        <AFFSUBID5></AFFSUBID5>
        <REFERRINGURL>'.$_POST["REFERRINGURL"].'</REFERRINGURL>
        <MINPRICE>0</MINPRICE>
    </REFERRAL>
    <CUSTOMER>
        <PERSONAL>
            <IPADDRESS>' . $_SERVER['REMOTE_HOST'] . '</IPADDRESS>
            <REQUESTEDAMOUNT>'.$_POST["REQUESTEDAMOUNTMIN"].'</REQUESTEDAMOUNT>
            <SSN>' . $_POST["SSN"] . '</SSN>
            <DOB>' . $_POST["DOB"] . '</DOB>
            <FIRSTNAME>' . $_POST["FIRSTNAME"] . '</FIRSTNAME>
            <LASTNAME>' . $_POST["LASTNAME"] . '</LASTNAME>
            <ADDRESS>' . $_POST["ADDRESS"] . '</ADDRESS>
            <CITY>' . $_POST["CITY"] . '</CITY>
            <STATE>' . $_POST["STATE"] . '</STATE>
            <ZIP>' . $_POST["ZIP"] . '</ZIP>
			<HOMEPHONE>' . $_POST["MOBILEPHONE"] . '</HOMEPHONE>
			<CELLPHONE>' . $_POST["CELLPHONE"] . '</CELLPHONE>
			<DLSTATE>' . $_POST["DLSTATE"] . '</DLSTATE>
			<DLNUMBER>' . $_POST["DLNUMBER"] . '</DLNUMBER>
			<ARMEDFORCES>' . $_POST["ARMEDFORCES"] . '</ARMEDFORCES>
			<CONTACTTIME>' . $_POST["CONTACTTIME"] . '</CONTACTTIME>
			<RENTOROWN>' . $_POST["HOMEOWNER"] . '</RENTOROWN>
			<EMAIL>' . $_POST["EMAIL"] . '</EMAIL>
            <ADDRESSMONTH>24</ADDRESSMONTH>
            <CITIZENSHIP></CITIZENSHIP>
            <OWNCAR>' . $_POST["CAROWNER"] . '</OWNCAR>
            <TCPAOPTIN></TCPAOPTIN>
            <USERAGENT>'.$_SERVER['HTTP_USER_AGENT'].'</USERAGENT>
        </PERSONAL>
        <EMPLOYMENT>
            <INCOMETYPE>' . $_POST["INCOMETYPE"] . '</INCOMETYPE>
            <PAYTYPE></PAYTYPE>
            <EMPTIME>' . $_POST["EMPTIME"] . '</EMPTIME>
            <EMPNAME>' . $_POST["EMPNAME"] . '</EMPNAME>
            <EMPPHONE>' . $_POST["EMPPHONE"] . '</EMPPHONE>
            <JOBTITLE>' . $_POST["JOBTITLE"] . '</JOBTITLE>
            <PAYFREQUENCY>' . $_POST["PAYFREQUENCY"] . '</PAYFREQUENCY>
            <NEXTPAYDATE>' . $_POST["NEXTPAYDATE"] . '</NEXTPAYDATE>
            <SECONDPAYDATE>' . $_POST["NEXTPAYDATE"] . '</SECONDPAYDATE>
        </EMPLOYMENT>
        <BANK>
            <BANKNAME>' . $_POST["BANKNAME"] . '</BANKNAME>
            <BANKPHONE></BANKPHONE>
            <ACCOUNTTYPE>' . $_POST["ACCOUNTTYPE"] . '</ACCOUNTTYPE>
            <ROUTINGNUMBER>' . $_POST["ROUTINGNUMBER"] . '</ROUTINGNUMBER>
            <ACCOUNTNUMBER>' . $_POST["ACCOUNTNUMBER"] . '</ACCOUNTNUMBER>
            <BANKMONTHS></BANKMONTHS>
            <NETMONTHLYINCOME>' . $_POST["NETMONTHLYINCOME"] . '</NETMONTHLYINCOME>
            <DIRECTDEPOSIT>' . $_POST["DIRECTDEPOSIT"] . '</DIRECTDEPOSIT>
        </BANK>
    </CUSTOMER>
</REQUEST>';
/*
print_r($xmpStr);
exit;
' . $_POST["INCOMETYPE"] . '
*/

/*
    $xmpStr =
    '<REQUEST>
	<REFERRAL>
		<CHANNELID>63395b8</CHANNELID>
		<PASSWORD>4fad7e6d</PASSWORD>
		<AFFSUBID>7937</AFFSUBID>
		<AFFSUBID2>980890</AFFSUBID2>
		<REFERRINGURL>www.leadz.aa</REFERRINGURL>
		<MINPRICE>4</MINPRICE>
	</REFERRAL>
	<CUSTOMER>
		<PERSONAL>
			<IPADDRESS>8.8.8.8</IPADDRESS>
			<REQUESTEDAMOUNT>'.$_POST["REQUESTEDAMOUNTMIN"].'</REQUESTEDAMOUNT>
			<SSN>' . $_POST["SSN"] . '</SSN>
			<DOB>' . $_POST["DOB"] . '</DOB>
            <FIRSTNAME>' . $_POST["FIRSTNAME"] . '</FIRSTNAME>
            <LASTNAME>' . $_POST["LASTNAME"] . '</LASTNAME>
            <ADDRESS>' . $_POST["ADDRESS"] . '</ADDRESS>
            <CITY>' . $_POST["CITY"] . '</CITY>
            <STATE>' . $_POST["STATE"] . '</STATE>
            <ZIP>' . $_POST["ZIP"] . '</ZIP>
			<HOMEPHONE>' . $_POST["MOBILEPHONE"] . '</HOMEPHONE>
			<CELLPHONE>' . $_POST["CELLPHONE"] . '</CELLPHONE>
			<DLSTATE>' . $_POST["DLSTATE"] . '</DLSTATE>
			<DLNUMBER>' . $_POST["DLNUMBER"] . '</DLNUMBER>
			<ARMEDFORCES>' . $_POST["ARMEDFORCES"] . '</ARMEDFORCES>
			<CONTACTTIME>' . $_POST["CONTACTTIME"] . '</CONTACTTIME>
			<RENTOROWN>' . $_POST["HOMEOWNER"] . '</RENTOROWN>
			<EMAIL>' . $_POST["EMAIL"] . '</EMAIL>
			<ADDRESSMONTH>24</ADDRESSMONTH>
			<CITIZENSHIP>yes</CITIZENSHIP>
		</PERSONAL>
		<EMPLOYMENT>
            <INCOMETYPE>' . $_POST["INCOMETYPE"] . '</INCOMETYPE>
            <EMPTIME>' . $_POST["EMPTIME"] . '</EMPTIME>
            <EMPNAME>' . $_POST["EMPNAME"] . '</EMPNAME>
            <EMPPHONE>' . $_POST["EMPPHONE"] . '</EMPPHONE>
            <JOBTITLE>' . $_POST["JOBTITLE"] . '</JOBTITLE>
            <PAYFREQUENCY>' . $_POST["PAYFREQUENCY"] . '</PAYFREQUENCY>
            <NEXTPAYDATE>' . $_POST["NEXTPAYDATE"] . '</NEXTPAYDATE>
            <SECONDPAYDATE></SECONDPAYDATE>
        </EMPLOYMENT>
		<BANK>
            <BANKNAME>' . $_POST["BANKNAME"] . '</BANKNAME>
            <BANKPHONE></BANKPHONE>
            <ACCOUNTTYPE>' . $_POST["ACCOUNTTYPE"] . '</ACCOUNTTYPE>
            <ROUTINGNUMBER>' . $_POST["ROUTINGNUMBER"] . '</ROUTINGNUMBER>
            <ACCOUNTNUMBER>' . $_POST["ACCOUNTNUMBER"] . '</ACCOUNTNUMBER>
            <BANKMONTHS></BANKMONTHS>
            <NETMONTHLYINCOME>' . $_POST["NETMONTHLYINCOME"] . '</NETMONTHLYINCOME>
            <DIRECTDEPOSIT>' . $_POST["DIRECTDEPOSIT"] . '</DIRECTDEPOSIT>
        </BANK>
	</CUSTOMER>
</REQUEST>';
*/  
    // $url = "https://formz.azurewebsites.net/import/"; //POST URL
    $url = "https://proffiliant.adrack.com/Import/"; //POST URL

    $ch = curl_init();
    if (!$ch) {
            die("Couldn't initialize a cURL handle");
        }
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $xmpStr);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

    $response = curl_exec($ch);
    curl_close($ch);
    
    // print_r($xmpStr);
    echo $response;
?>