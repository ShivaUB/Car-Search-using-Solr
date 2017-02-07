    $(document).ready(function () {
        var makeDDL = $('#make');
        var modelDDL = $('#model');
        var selectedMake = "";
        var selectedModel = "";

        $.ajax({
            url: 'Home/SearchCars',
            method: 'post',
            dataType: 'json',
            success: function (data) {
                console.log(data);
                $(data).each(function (index, item) {
                    d = document.createElement('div');
                    $(d).addClass("element-item")
                        .html("<p> Make : " + item.Make[0] + "</p> <p> Model : " + item.Model[0] + "</p>  <p>Year : " + item.Year + "</p><p> HorsePower : " + item.HorsePower + "hp </p><p> TopSpeed : " + item.TopSpeed + "mph </p><p> Price : $" + item.Price + ".00 </p><hr/> <a href=\"" + item.ImageUrl[0] + "\"> <img  src=\"" + item.ImageUrl[0] + "\" height=100 width=150 /></a></div><br/> <hr/>")
                        .appendTo($("#grid"));
                })
            },
            error: function (err) {
                alert(err);
            }
        });



        $.ajax({
            url: 'Home/GetMake',
            method: 'post',
            dataType: 'json',
            success: function (data) {
                makeDDL.append($('<option/>', { value: 'Select Make', text: 'Select Make' }));
                modelDDL.append($('<option/>', { value: 'Select Model', text: 'Select Model' }));
                modelDDL.prop('disabled', true);

                $(data).each(function (index, item) {
                    makeDDL.append($('<option/>', { value: item, text: item }));
                });
            },
            error: function (err) {
                alert(err);
            }
        });

        makeDDL.change(function () {
            selectedMake=$(this).val();
            if ($(this).val() == "Select Make") {
                modelDDL.empty();
                modelDDL.append($('<option/>', { value: 'Select Model', text: 'Select Model' }));
                modelDDL.val('Select Model');
                modelDDL.prop('disabled', true);
            }
            else {
                $.ajax({
                    url: 'Home/GetModel',
                    method: 'post',
                    dataType: 'json',
                    data: { MakeName: $(this).val() },
                    success: function (data) {
                        modelDDL.empty();
                        modelDDL.append($('<option/>', { value: 'Select Model', text: 'Select Model' }));
                        $(data).each(function (index, item) {
                            modelDDL.append($('<option/>', { value: item, text: item }));
                        });
                        modelDDL.val('Select Model');
                        modelDDL.prop('disabled', false);
                    },
                    error: function (err) {
                        alert(err);
                    }
                });
            }
        });

        modelDDL.change(function () {
            selectedModel = $(this).val();
            $.ajax({
                url: 'Home/GetCars',
                method: 'post',
                dataType: 'json',
                data: {  Model: $(this).val(), Make: selectedMake },
                success: function (data) {
                    console.log(data);
                    $("#grid").empty();
                    $(data).each(function (index, item) {
                        console.log(item);
                        d = document.createElement('div');
                        $(d).addClass("element-item")
                            .html("<p> Make : " + item.Make[0] + "</p> <p> Model : " + item.Model[0] + "</p>  <p>Year : " + item.Year + "</p><p> HorsePower : " + item.HorsePower + "hp </p><p> TopSpeed : " + item.TopSpeed + "mph </p><p> Price : $" + item.Price + ".00 </p><hr/> <a href=\"" + item.ImageUrl[0] + "\"> <img  src=\"" + item.ImageUrl[0] + "\" height=100 width=150 /></a></div><br/> <hr/>")
                        .appendTo($("#grid"));
                    })
                },
                error: function (err) {
                    alert(err);
                }
            });
        });
    
        $('.quicksearch').keyup(function () {
            $("#grid").empty();
            $.ajax({
                url: 'Home/SearchCars',
                method: 'post',
                dataType: 'json',
                data: { searchKey: $(".quicksearch").val() },
                success: function (data) {
                    console.log(data);
                    $(data).each(function (index, item) {
                        d = document.createElement('div');
                        $(d).addClass("element-item")
                            .html("<p> Make : " + item.Make[0] + "</p> <p> Model : " + item.Model[0] + "</p>  <p>Year : " + item.Year + "</p><p> HorsePower : " + item.HorsePower + "hp </p><p> TopSpeed : " + item.TopSpeed + "mph </p><p> Price : $" + item.Price + ".00 </p><hr/> <a href=\"" + item.ImageUrl[0] + "\"> <img  src=\"" + item.ImageUrl[0] + "\" height=100 width=150 /></a></div><br/> <hr/>")
                         .appendTo($("#grid"));
                    })
                },
                error: function (err) {
                    alert(err);
                }
            });
        });
    });