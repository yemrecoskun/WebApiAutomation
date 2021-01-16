$(document).ready(function () {
    var modal = document.getElementById('addendpoint');
    var sortList;
    var key = GetRandomString(16);
    $("#endpointBtn").click(function () {
        EndpointAdd();
    });
    $("#addSortBtn").click(function () {
        SortAdd();
    });
    $("#CloseModal").click(function () {
        CloseModal();
    });
    $("#CloseModalBtn").click(function () {
        CloseModal();
    });
    $("#closeaddsortmodal").click(function () {
        CloseAddSortModal();
    });
    $("#closeAddSortModalBtn").click(function () {
        CloseAddSortModal();
    });
    $("#openSortBtn").click(function () {
        openSortBtn();
    });
    $("#openEndpointBtn").click(function () {
        openEndpointBtn();
    });
    $("#addEndpointBtn").click(function () {
        addEndpointSort();
    });
    $("#addEndpointModal").click(function () {
        document.getElementById('addendpoint').style.display = 'block';
    });
    $("#addSortModal").click(function () {
        document.getElementById('addsort').style.display = 'block';
    });
    $("#messageModalClose").click(function () {
        document.getElementById('messageModal').style.display = 'none';
    });
    $("#clearBtn").click(function () {
        $('h4#consoleValueShop').remove();
    });
    function EndpointAdd() {
        var ServiceName = $('#ServiceName').val();
        var EndPoint = $('#EndPoint').val();
        var Request = $('#Request').val();
        var Header = $('#Header').val();
        var RequestType = $('#RequestType').val();
        var Action = $('#Action').val();
        $.ajax({
            url: '../Endpoint/PostEndpoint',
            type: 'POST',
            dataType: 'json',
            data: { ServiceName: ServiceName, EndPoint: EndPoint, Request: Request, Header: Header, RequestType: RequestType, Action: Action },
            success: function (exception) {
                if (exception != "") {
                    $('#console').append("<h4 id='consoleValueShop'> >_" + exception + "</h4>");
                }
                else {
                    window.location.reload();
                }
            }
        })
    }
    function SortAdd() {
        var table = document.getElementById("sortTable");
        var tbody = table.getElementsByTagName("tbody");
        var Id = new Array();
        for (var i = 0; i < tbody.length; i++) {
            Id.push(tbody[i].getElementsByTagName("th")[0].textContent);
        }
        var sortName = $("#addSortName").val();
        $.ajax({
            url: '../Endpoint/PostSort',
            type: 'POST',
            dataType: 'json',
            data: { Id: Id, sortName: sortName },
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
    function CloseModal() {
        $('#ServiceName').val('');
        $('#EndPoint').val('');
        $('#RequestType').val('0');
        $('#Action').val('');
        $('#Request').val('{\n\"\": \"\"\n}');
        $('#Header').val('{\n\"\": \"\"\n}');
        document.getElementById('addendpoint').style.display = 'none';
    }
    function CloseAddSortModal() {
        document.getElementById('addsort').style.display = 'none';
        var table = document.getElementById("sortTable");
        var tbody = table.getElementsByTagName("tbody");
        $(tbody).remove();
    }
    function openSortBtn() {
        $("#sortDiv").remove();
        var sortName = $("Select#sort").val();
        $.ajax({
            url: '../Endpoint/GetSortList',
            type: 'POST',
            dataType: 'json',
            data: { sortName: sortName },
            success: function (resultList) {
                sortList = resultList;
                $("#sortContainer").append('<div id="sortDiv"></div>');
                $("#sortDiv").append('<center><span id="sortSpan">' + sortName + '</span></center><br/>');
                $("#sortDiv").append('<center><input class="form-control" type="text" id="url" value=' + resultList[0].ServiceName + '/></center><br/>');
                $("#sortDiv").append('<div class="row" id="endPointListModal"></div>');
                for (var i = 0; i < sortList.length; i++) {
                    if (sortList[i].RequestType == 1) {
                        var end = 'end' + sortList[i].Id;
                        $("#endPointListModal").append('<div class="col-md-4" id="' + end + '1"></div>');
                        $("#" + end + "1").append('<div class="card mb-4 shadow-sm" id="' + end + '2"></div>');
                        $("#" + end + "2").append('<div class="card-body"><p class="card-text" id=Action' + sortList[i].Id + '>' + sortList[i].Action + '</p><span class="text-muted">POST</span></div>');
                        $("#" + end + "2").append('<div class= "card-body openSortEndpoint" id= "' + end + '3"><label>Header (Json Format)</label><textarea type="text" class="form-control" id="Header' + sortList[i].Id + '" placeholder="Header Json" >' + sortList[i].Header + '</textarea><label>Request (Json Format)</label><textarea type="text" class="form-control" id="Request' + sortList[i].Id + '" placeholder="Request Json" >' + sortList[i].Request + '</textarea></div >');
                    }
                    else {
                        var end = 'end' + sortList[i].Id;
                        $("#endPointListModal").append('<div class="col-md-4" id="' + end + '1"></div>');
                        $("#" + end + "1").append('<div class="card mb-4 shadow-sm" id="' + end + '2"></div>');
                        $("#" + end + "2").append('<div class="card-body"><p class="card-text" id=Action' + sortList[i].Id + '>' + sortList[i].Action + '</p><span class="text-muted">GET</span></div>');
                        $("#" + end + "2").append('<div class= "card-body openSortEndpoint" id= "' + end + '3"><label>Header (Json Format)</label><textarea type="text" class="form-control" id="Header' + sortList[i].Id + '" placeholder="Header Json" >' + sortList[i].Header + '</textarea><label>Request (Json Format)</label><textarea type="text" class="form-control" id="Request' + sortList[i].Id + '" placeholder="Request Json" disabled="disabled" >' + sortList[i].Request + '</textarea></div >');
                    }
                }
                $("#sortDiv").append('<center><button type="button" class="btn-group btn btn-sm btn-outline-primary" id="runBtn"> Run </button> <button type="button" class="btn-group btn btn-sm btn-outline-primary" id="removeBtn"> Clear </button></center>');
                $("#runBtn").click(function () {
                    runBtnClick();
                });
                $("#removeBtn").click(function () {
                    document.getElementById("sortDiv").remove();
                });
            }
        });
    }
    function openEndpointBtn() {
        $("#sortDiv").remove();
        var id = $("Select#SelectEndpoint").val();
        $.ajax({
            url: '../Endpoint/GetEndpoint',
            type: 'POST',
            dataType: 'json',
            data: { id: id },
            success: function (endpoint) {
                $("#sortContainer").append('<div id="sortDiv"></div>');
                $("#sortDiv").append('<center><input class="form-control" type="text" id="url" value=' + endpoint.ServiceName + '/></center><br/>');
                $("#sortDiv").append('<div class="row" id="endPointListModal"></div>');
                if (endpoint.RequestType == 1) {
                    var end = 'end' + endpoint.Id;
                    $("#endPointListModal").append('<div class="col-md-4" id="' + end + '1"></div>');
                    $("#" + end + "1").append('<div class="card mb-4 shadow-sm" id="' + end + '2"></div>');
                    $("#" + end + "2").append('<div class="card-body"><p class="card-text" id=Action' + endpoint.Id + '>' + endpoint.Action + '</p><span class="text-muted">POST</span></div>');
                    $("#" + end + "2").append('<div class= "card-body openSortEndpoint" id= "' + end + '3"><label>Header (Json Format)</label><textarea type="text" class="form-control" id="Header' + endpoint.Id + '" placeholder="Header Json" >' + endpoint.Header + '</textarea><label>Request (Json Format)</label><textarea type="text" class="form-control" id="Request' + endpoint.Id + '" placeholder="Request Json" >' + endpoint.Request + '</textarea></div >');
                }
                else {
                    var end = 'end' + endpoint.Id;
                    $("#endPointListModal").append('<div class="col-md-4" id="' + end + '1"></div>');
                    $("#" + end + "1").append('<div class="card mb-4 shadow-sm" id="' + end + '2"></div>');
                    $("#" + end + "2").append('<div class="card-body"><p class="card-text" id=Action' + endpoint.Id + '>' + endpoint.Action + '</p><span class="text-muted">GET</span></div>');
                    $("#" + end + "2").append('<div class= "card-body openSortEndpoint" id= "' + end + '3"><label>Header (Json Format)</label><textarea type="text" class="form-control" id="Header' + endpoint.Id + '" placeholder="Header Json" >' + endpoint.Header + '</textarea><label>Request (Json Format)</label><textarea type="text" class="form-control" id="Request' + endpoint.Id + '" placeholder="Request Json" disabled="disabled" >' + endpoint.Request + '</textarea></div >');
                }
                $("#sortDiv").append('<center><button type="button" class="btn-group btn btn-sm btn-outline-primary" id="runBtn"> Run </button> <button type="button" class="btn-group btn btn-sm btn-outline-primary" id="removeBtn"> Clear </button></center>');
                $("#runBtn").click(function () {
                    runBtnClick();
                });
                $("#removeBtn").click(function () {
                    document.getElementById("sortDiv").remove();
                });
            }
        });
    }
    function runBtnClick() {
        var end = [];
        var action = [];
        var header = [];
        var request = [];
        for (var i = 0; i < sortList.length; i++) {
            end[i] = 'end' + sortList[i].Id;
            action[i] = $("#Action" + sortList[i].Id).text();
            header[i] = $("#Header" + sortList[i].Id).val();
            request[i] = $("#Request" + sortList[i].Id).val();
        }
        var url = $("#url").val();

        $("#sortDiv").hide();
        $("#sortContainer").append('<center id="loader"><div class="loader"></div></center>');
        $("#runBtn").remove();
        $.ajax({
            url: '../Endpoint/RequestClient',
            type: 'POST',
            dataType: 'json',
            data: { url: url, ActionName: action, header: header, request: request, key: key },
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $("#sortDiv").show();
                    $("#loader").remove();
                    var a = "#" + end[i] + "3";
                    $(a).remove();
                    $("#" + end[i] + "2").append('<div class="d-flex justify-content-between align-items-center" id="response' + i + '"></div>');
                    var endId = "'" + end[i].substr(3) + "'";
                    $("#response" + i).append('<div class="btn-group"><button type="button" class="btn btn-sm btn-outline-primary requestBtn" id=' + endId + '>Request</button><button type="button" class="btn btn-sm btn-outline-primary responseBtn" id=' + endId + '>Response</button></div>');
                    $(".requestBtn").click(function () {
                        requestClick(this.id);
                    });
                    $(".responseBtn").click(function () {
                        responseClick(this.id);
                    });
                    if (data[i] == "Success") {
                        $("#response" + i).append('<small class="text-muted"><svg class="checkmark" viewBox="0 0 52 52"><circle class="checkmark__circle" cx="26" cy="26" r="25" fill="none" /><path class="checkmark__check" fill="none" d="M14.1 27.2l7.1 7.2 16.7-16.8" stroke-width="5" /></svg></small>');
                    }
                    else {
                        $("#response" + i).append('<small class="text-muted"><svg class="checkmark1" viewBox="0 0 52 52"><circle class="checkmark__circle1" cx="26" cy="26" r="25" fill="none" /> <path class="checkmark__check1" fill="none" d="M16 16 36 36 M36 16 16 36" stroke-width="5" /></svg></small>');
                    }
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
                $("#sortTable").append('<tbody id=endpoint' + endpoint.Id + '><tr><th scope="row">' + endpoint.Id + '</th><td>' + endpoint.Action + '</td><td>' + endpoint.EndPoint + '</td><td> <span class="w3-button removeEndpointClass" id=' + Id + '>&times;</span></td></tr></tbody>');
                $(".removeEndpointClass").click(function () {
                    $("tbody#" + this.id).remove();
                });
            }
        })
    }
    function responseClick(id) {
        $("#message").empty();
        $.ajax({
            url: '../Endpoint/GetResponseMessage',
            type: 'POST',
            dataType: 'json',
            data: { id: id, key: key },
            success: function (data) {
                document.getElementById('messageModal').style.display = 'block';
                $("#message").text(data);
            }
        })
    }
    function requestClick(id) {
        $("#message").empty();
        $.ajax({
            url: '../Endpoint/GetRequestMessage',
            type: 'POST',
            dataType: 'json',
            data: { id: id, key: key },
            success: function (data) {
                document.getElementById('messageModal').style.display = 'block';
                $("#message").text(data);
            }
        })
    }
    function GetRandomString(length) {
        var randomChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var result = '';
        for (var i = 0; i < length; i++) {
            result += randomChars.charAt(Math.floor(Math.random() * randomChars.length));
        }
        return result;
    }
});