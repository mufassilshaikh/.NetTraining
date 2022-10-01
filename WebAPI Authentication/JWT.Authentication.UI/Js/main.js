$(document).ready(function(){

    $("#register").click(function(){
        var fullName = $('#fullName').val();
        var email = $("#email").val();
        var password = $("#password").val();
        var payload = {
            "FullName": fullName,
            "Email": email,
            "Password": password
        }
        var url = 'https://localhost:7021/api/Auth/register'
       
        $.ajax
        ({ 
            headers: { 
                'Accept': 'application/json',
                'Content-Type': 'application/json' 
            },
            url: url,
            data: JSON.stringify(payload),
            type: 'post',
            success: function(result)
            {
               alert('User Registered Successfully');
            }
        });
    })

    $("#login").click(function(){
        var email = $("#email").val();
        var password = $("#password").val();
        var payload = {
            "Email": email,
            "Password": password
        }
        var url = 'https://localhost:7021/api/Auth/login'
       
        $.ajax
        ({ 
            headers: { 
                'Accept': 'application/json',
                'Content-Type': 'application/json' 
            },
            url: url,
            data: JSON.stringify(payload),
            type: 'post',
            success: function(result)
            {
                localStorage.setItem('token', result);
                alert('Logged in Successfully');
                window.location.href = 'index.html';
            }
        });
    })

})