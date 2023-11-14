var _appModule = undefined;

(function (module) //interjú modul 
{
    $(document).ready(function ()
    {
        var loading = $(`<div class="loading"><div id="loading-bar-spinner" class="spinner"><div class="spinner-icon"></div></div></div>`);
        //Nem véletlenül vanak itt ezek
        $(document).ajaxStart(function (e)
        {
            $("#listContainer").html(loading);
            console.log("ajax start");
        }).ajaxStop(function (e)
        {
            console.log("ajax stop");
        });

        let keyupTimer;
        $(".search").keypress(function () {
            clearTimeout(keyupTimer);
            keyupTimer = setTimeout(function () {
                module.LoadList();
            }, 2000);
        });

        //Early binding a rendezéshez 
        $(document).on("click", ".sortedcol", function ()
        {
            var originalCol = $("#SortCol").val();
            var order = $("#SortOrder");
            var currentCol = $(this).attr("data-sortorder");
            $("#SortCol").val(currentCol);
            if(originalCol != currentCol)
            {
                //Ha másik oszlopot választ, akkor alapértelmezett növekvő
                order.val('1');
            }
            else
            {
                //Unsorted->Asc->Desc->Unsorted rotáció
                var currentOrder = order.val();
                if (currentOrder == module.SortOrerEnum.Unsorted) {
                    order.val(module.SortOrerEnum.Asc);
                }
                else if (currentOrder == module.SortOrerEnum.Asc) {
                    order.val(module.SortOrerEnum.Desc);
                }
                else if (currentOrder == module.SortOrerEnum.Desc)
                {
                    order.val(module.SortOrerEnum.Unsorted);
                    $("#SortCol").val('');
                }
            }
            module.LoadList();
        });

        //Early binding a lapozáshoz
        $(document).on("click", ".pagenum", function ()
        {
            $("#PageIndex").val($(this).attr("data-pageindex"));
            module.LoadList();
        });

        //Első betöltés
        module.LoadList();
    });

    //konstansok
    module.SortOrerEnum = {Unsorted: 0,Asc: 1, Desc:2}
    
    //Lista betöltése
    module.LoadList = function()
    {
        //model builder
        var model = { PageIndex: $("#PageIndex").val(), SortOrder: $("#SortOrder").val(), SortCol: $("#SortCol").val(), SearchVal: $(".search").val() }
        $.ajax({
            url: "/Home/LoadListPartial",
            type: 'GET',
            data: model,
            success: function (data)
            {
                //Replace data
                $("#listContainer").html(data);
            }
        });
    }
})(_appModule || (_appModule = {}));
