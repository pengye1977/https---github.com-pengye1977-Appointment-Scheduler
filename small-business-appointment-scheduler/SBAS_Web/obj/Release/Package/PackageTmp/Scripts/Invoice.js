function InjectAddInvoice() {
    $.ajax({
        type: "GET",
        url: "/Account/AddInvoice",
        cache: false,
        success: function (result) {
            $("#DisplayArea").html(result);
        },
        error: function (result) {
            alert(result);
        }
    });
}
