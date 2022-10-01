$(document).ready(function(){
    var url = 'https://localhost:7061/WeatherForecast'
    $.ajax
    ({ 
        headers: { 
            'Accept': 'application/json',
            'Content-Type': 'application/json', 
            'Authorization' : 'bearer ' + localStorage.getItem('token')
        },
        url: url,
        type: 'get',
        success: function(result)
        {
            for(var item of result)
            {
                $('.tableContent').append(`<tr>
                    <td>${moment(item.date).format("MM/DD/YYYY")}</td>
                    <td>${item.temperatureC}</td>
                    <td>${item.temperatureF}</td>
                    <td>${item.summary}</td>
                </tr>`);
            }
            $('.table').css("display", "block");;
        },
        error: function(xhr, error) {
            var status = xhr.status; 
            console.log(status)
            if(status==401){
                alert('Unauthorized user');
                window.location.href = 'login.html';
            }
            else
                alert('Failed to get results');
        }
    });
});
