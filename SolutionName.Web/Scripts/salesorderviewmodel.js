/// <reference path="knockout.mapping-latest.js" />
/// <reference path="knockout-3.1.0.js" />
var ObjectState = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3
}

var salesOrderItemMapping = {
    'SalesOrderItems': {
        key: function (salesOrderItem) {
            return ko.utils.unwrapObservable(salesOrderItem.SalesOrderItemId);
        },
        create: function (options) {
            return new SalesOrderItemViewModel(options.data);
        }
    }
}

var dataConverter = function (key, value) {
    if (key === 'RowVersion' && Array.isArray(value)) {
        var str = String.fromCharCode.apply(null, value);
        return btoa(str);
    }

    return value;
}

SalesOrderItemViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.flagSalesOrderItemAsEdited = function () {
        if (self.ObjectState() != ObjectState.Added) {
            self.ObjectState(ObjectState.Modified);
        }
        return true;
    };

    self.ExtendedPrice = ko.computed(function () {
        return (self.Quantity() * self.UnitPrice()).toFixed(2);
    });
}


SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping , self);

    self.save = function () {
        $.ajax({
            url: "/SalesOrders/Save",
            type: "POST",
            data: ko.toJSON(self, dataConverter),
            contentType: "application/json",
            success: function (data) {
                if (data.salesOrderViewModel != null)
                    ko.mapping.fromJS(data.salesOrderViewModel, {}, self);
                if (data.newLocation != null)
                    window.location = data.newLocation;
            }
        })
    };

    self.flagSalesOrderAsEdited = function () {
        if (self.ObjectState() != ObjectState.Added)
            self.ObjectState(ObjectState.Modified);
        return true;
    };

    self.addSalesOrderItem = function () {
        var salesOrderItem = new SalesOrderItemViewModel({ SalesOrderItemId: 0, ProductCode: "", Quantity: 1, UnitPrice: 0, ObjectState: ObjectState.Added, ServiceTypes: self.ServiceTypes, ServiceTypeId: 1, ExtendWarranty: false});
        self.SalesOrderItems.push(salesOrderItem);
    };

    self.Total = ko.computed(function () {
        var total = 0;
        ko.utils.arrayForEach(self.SalesOrderItems(), function (salesOrderItem) {
            total += parseFloat(salesOrderItem.ExtendedPrice());
        });
        return total.toFixed(2);
    });

    self.deleteSalesOrderItem = function (salesOrderItem) {
        self.SalesOrderItems.remove(this);
        if (salesOrderItem.SalesOrderItemId() > 0 && self.SalesOrderItemsToDelete.indexOf(salesOrderItem.SalesOrderItemId()) == -1)
            self.SalesOrderItemsToDelete.push(salesOrderItem.SalesOrderItemId());
    };
}
$("form").validate({
    submitHandler: function () {
        salesOrderViewModel.save();
    },
    rules: {
        CustomerName: {
            required: true,
            maxlength: 30
        },
        PONumber: {
            maxlength: 10
        },
        CityId:{
            required: true
        },
        ProductCode: {
            required: true,
            maxlength: 15,
            alphaonly: true
        },
        Quantity: {
            required: true,
            digits: true,
            range: [1, 1000000]
        },
        UnitPrice: {
            required: true,
            number: true,
            range: [0, 100000]
        }
    },
    messages: {
        CustomerName: {
            required: "You cannot create a sales order unless you supply the customer's name.",
            maxlength: "Customer names must be 30 characters or shorter."
        },
        ProductCode: {
            alphaonly: "Product codes consist of letters only."
        }
    }
});



$.validator.addMethod("alphaonly",
    function (value) {
        return /^[A-Za-z]+$/.test(value);
    }
);