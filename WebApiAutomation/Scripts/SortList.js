$(document).ready(function () {
    var sortName;
    $("#closeupdatesortmodal").click(function () {
        CloseModal();
    });
    $("#CloseModalBtn").click(function () {
        CloseModal();
    });
    $("#UpdateButton").click(function () {
        UpdateSort();
    });
    $("#addEndpointBtn").click(function () {
        addEndpointSort();
    });
    $(".sortDeleteBtn").click(function () {
        Delete(this.id);
    });
    $(".sortUpdateBtn").click(function () {
        GetUpdate(this.id);
    });
    $("#clearBtn").click(function () {
        $('h4#consoleValueShop').remove();
    });
});
function Delete(SortName) {
    if (confirm("Sil") == true) {
        $.ajax({
            url: '../Endpoint/DeleteSort',
            type: 'POST',
            dataType: 'json',
            data: { SortName: SortName },
            success: function (data) {
                if (data == "") {
                    $("tbody#" + SortName).remove();
                }
                else {
                    $('#console').append("<h4 id='consoleValueShop'> >_" + data + "</h4>");
                }
            }
        })
    }
}
function GetUpdate(SortName) {
    $.ajax({
        url: '../Endpoint/GetUpdateSort',
        type: 'POST',
        dataType: 'json',
        data: { SortName: SortName },
        success: function (endpoint) {
            try {
                sortName = SortName;
                document.getElementById('updatesort').style.display = 'block';
                for (var i = 0; i < endpoint.length; i++) {
                    document.getElementById("updateSortName").value = SortName;
                    var Id = "endpoint" + endpoint[i].Id + "";
                    $("#endTable").append('<tbody id=endpoint' + endpoint[i].Id + '><tr><th scope="row">' + endpoint[i].Id + '</th><td>' + endpoint[i].Action + '</td><td>' + endpoint[i].EndPoint + '</td><td> <span class="w3-button removeEndpointClass" id=' + Id + '>&times;</span></td></tr></tbody>');
                    $(".removeEndpointClass").click(function () {
                        $("tbody#" + this.id).remove()
                    });
                }
            }
            catch{
                $('#console').append("<h4 id='consoleValueShop'> >_" + endpoint + "</h4>");
            }
        }
    })
}
function CloseModal() {
    document.getElementById('updatesort').style.display = 'none';
    var table = document.getElementById("endTable");
    var tbody = table.getElementsByTagName("tbody");
    $(tbody).remove();
}
function UpdateSort() {
    var table = document.getElementById("endTable");
    var tbody = table.getElementsByTagName("tbody");
    var Id = new Array();
    for (var i = 0; i < tbody.length; i++) {
        Id.push(tbody[i].getElementsByTagName("th")[0].textContent);
    }
    var SortName = $("#updateSortName").val();
    $.ajax({
        url: '../Endpoint/UpdateSort',
        type: 'POST',
        dataType: 'json',
        data: { Id: Id, newsortName: SortName, sortName: sortName },
        success: function (exception) {
            if (exception != "") {
                $('#console').append("<h4 id='consoleValueShop'> >_" + exception + "</h4>");
            }
            else {
                window.location.reload();
            }
        }
    });
}
function addEndpointSort() {
    var id = $("#endpointaddtosort").val();
    $.ajax({
        url: '../Endpoint/GetEndpoint/' + id,
        type: 'POST',
        text: 'json',
        success: function (endpoint) {
            var Id = "endpoint" + endpoint.Id + "";
            $("#endTable").append('<tbody id=endpoint' + endpoint.Id + '><tr><th scope="row">' + endpoint.Id + '</th><td>' + endpoint.Action + '</td><td>' + endpoint.EndPoint + '</td><td> <span class="w3-button removeEndpointClass" id=' + Id + '>&times;</span></td></tr></tbody>');
            $(".removeEndpointClass").click(function () {
                $("tbody#" + this.id).remove()
            });
        }
    })
}