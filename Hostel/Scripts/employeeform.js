function validation(){
	
	var name=nameValidation('name','nameErr');
	var email=emailValidation('email','emailErr');
	var phone=phoneValidation('phone','phoneErr');
	
	if(name==true && email==true && phone==true ){
		return true;
	}
	else{
		return false;
	}
}