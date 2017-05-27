function check() {

    if (sign.userName.value == "") {
        alert("請輸入姓名");

    };
    //欄位名稱: 1. username id名稱: 1.text[0]
    //2. password 2.text[1]
    //3. re_password(再次確認密碼) 3.text[2]
    //4. chname(姓名) 4.text[3]
    //5. tel 5.text[4]
    //6. address 6.text[5]
    //7. mail 7.text[6]
   

        function check_item() {

            for (i = 0; i < 7; i++) {
                if(document.register.elements["text["+i+"]"].value=="") {

                    while (i < 7) {
                        if (i == 0)
                            alert("申請帳號欄位不能空白");
                        else if (i == 1)
                            alert("申請密碼欄位不能空白");
                        else if (i == 2)
                            alert("確認密碼欄位不能空白");
                        else if (i == 3)
                            alert("申請姓名欄位不能空白");
                        else if (i == 4)
                            alert("申請電話欄位不能空白");
                        else if (i == 5)
                            alert("申請地址欄位不能空白");
                        else if (i == 6)
                            alert("申請信箱欄位不能空白");

                    }
                    return false;
                }

            }


        }
}