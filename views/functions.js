function checkToken(){
    if(sessionStorage.getItem("token"))
      showApplication();
  }