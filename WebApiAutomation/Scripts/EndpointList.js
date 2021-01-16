$(document).ready(function () {
    $("#CloseModal").click(function () {
        CloseModal();
    });
    $("#CloseModalBtn").click(function () {
        CloseModal();
    });
    $("#UpdateEndpoint").click(function () {
        UpdateEndpoint();
    });
    $(".endpointDeleteBtn").click(function () {
        Delete(this.id);
    });
    $(".endpointUpdateBtn").click(function () {
        GetUpdate(this.id);
    });
    $("#clearBtn").click(function () {
        $('h4#consoleValueShop').remove();
    });
});

function Delete(Id) {
    $.ajax({
        url: '../Endpoint/DeleteEndpoint/' + Id,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data == "") {
                $("tbody#" + Id).remove();
            }
            else {
                $('#console').append("<h4 id='consoleValueShop'> >_" + data + "</h4>");
            }
        }
    })
}

function GetUpdate(Id) {
    $.ajax({
        url: '../Endpoint/GetUpdateEndpoint/' + Id,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            try {
                $('#ServiceName').val(data.ServiceName);
                $('#EndPoint').val(data.EndPoint);
                $('#RequestType').val(data.RequestType);
                $('#Action').val(data.Action);
                $('#Request').val(data.Request);
                $('#Header').val(data.Header);
                $('#EndpointId').val(data.Id);
                document.getElementById('updateendpoint').style.display = 'block';
            }
            catch{
                $('#console').append("<h4 id='consoleValueShop'> >_" + data + "</h4>");
            }
        }
    })
}

function CloseModal() {
    $('#ServiceName').val('');
    $('#EndPoint').val('');
    $('#RequestType').val('0');
    $('#Action').val('');
    $('#Request').val('{\n\"\": \"\"\n}');
    $('#Header').val('{\n\"\": \"\"\n}');
    document.getElementById('updateendpoint').style.display = 'none';
}

function UpdateEndpoint() {
    var ServiceName = $('#ServiceName').val();
    var EndPoint = $('#EndPoint').val();
    var Request = $('#Request').val();
    var Header = $('#Header').val();
    var RequestType = $('#RequestType').val();
    var Action = $('#Action').val();
    var Id = $("#EndpointId").val();
    $.ajax({
        url: '../Endpoint/UpdateEndpoint',
        type: 'POST',
        dataType: 'json',
        data: { Id: Id, ServiceName: ServiceName, EndPoint: EndPoint, Request: Request, Header: Header, RequestType: RequestType, Action: Action },
        success: function (exception) {
            if (exception != "") {
                $('#console').append("<h4 id='consoleValueShop'> >_" + exception + "</h4>");
                CloseModal();
            }
            else {
                window.location.reload();
            }
        }
    })
}