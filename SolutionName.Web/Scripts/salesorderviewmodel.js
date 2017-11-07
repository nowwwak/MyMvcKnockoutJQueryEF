/// <reference path="knockout.mapping-latest.js" />
/// <reference path="knockout-3.1.0.js" />
var ObjectState = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3

}

SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.save = function () {
        $.ajax({
            url: "/SalesOrders/Save",
            type: "POST",
            data: ko.toJSON(self),
            contentType: "application/json",
            success: function (data) {
                if (data.salesOrderViewModel != null)
                    ko.mapping.fromJS(data.salesOrderViewModel, {}, self);
            }
        })
    };

    self.flagSalesOrderAsEdited = function () {        
        ddif(self.ObjectState() != ObjectState.Added)
            self.ObjectState(ObjectState.Modified);
        return true;
    }
}