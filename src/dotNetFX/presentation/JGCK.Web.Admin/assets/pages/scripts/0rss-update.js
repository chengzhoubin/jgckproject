$(function () {

    $.fn.select2.defaults.set("theme", "bootstrap");

    //医院
    function formatRepo1(repo) {
        if (repo.loading) return repo.text;
        var markup = "<div class='select2-result-repository clearfix'><div class='select2-result-repository__meta'>" + repo.name + "</div></div>";
        return markup;
    }
    function formatRepoSelection1(repo) {
        console.log(repo);
        return repo.name||repo.text;
    }
    $("#select2-JoinStringOfHospitalName").select2({
        width: "off",
        ajax: {
            url: "res/data/hospital/asynclist",
            dataType: 'json',
            delay: 250,
            type: "POST",
            data: function (params) {
                return {
                    f: params.term
                };
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: data
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: 1,
        templateResult: formatRepo1,
        templateSelection: formatRepoSelection1
    });
    


    //职称
    function formatRepo3(repo) {
        if (repo.loading) return repo.text;
        return "<div class='select2-result-repository clearfix'><div class='select2-result-repository__meta'>" + repo.name + "</div></div>";
    }
    function formatRepoSelection3(repo) {
        return repo.name || repo.text;
    }
    $("#select2-JoinStringOfTitleName").select2({
        width: "off",
        ajax: {
            url: "res/data/pt/asynclist",
            dataType: 'json',
            delay: 250,
            type: "POST",
            data: function (params) {
                return {
                    filter: params.term
                };
            },
            processResults: function (data) {
                var _d = [];         
                _.each(data,function(item,i){
                    _d.push({ id: item.ID, name: item.TitleName });
               })           
                return {
                    results: _d
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: 1,
        templateResult: formatRepo3,
        templateSelection: formatRepoSelection3
    });



    //医生
    function formatRepo4(repo) {
        if (repo.loading) return repo.text;
        var markup = "<div class='select2-result-repository clearfix'><div class='select2-result-repository__meta'>" + repo.name + "</div></div>";
        return markup;
    }
    function formatRepoSelection4(repo) {
        return repo.name || repo.text;
    }
    $("#select2-DoctorList").select2({
        width: "off",
        ajax: {
            url: "res/data/doctor/asynclist",
            dataType: 'json',
            delay: 250,
            type: "POST",
            data: function (params) {
                return {
                    doctorName: params.term
                };
            },
            processResults: function (data) {
                var _d = [];
                _.each(data, function (item, i) {
                    _d.push({ id: item.ID, name: item.DoctorName });
                })
                return {
                    results: _d
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: 1,
        templateResult: formatRepo4,
        templateSelection: formatRepoSelection4
    });

    //病种
    function formatRepo2(repo) {
        if (repo.loading) return repo.text;
        var markup = "<div class='select2-result-repository clearfix'><div class='select2-result-repository__meta'>" + repo.name + "</div></div>";
        return markup;
    }
    function formatRepoSelection2(repo) {
        return repo.name || repo.text;
    }
    $("#select2-JoinStringOfDiseaselName").select2({
        width: "off",
        ajax: {
            url: "res/data/disease/asynclist",
            dataType: 'json',
            delay: 250,
            type: "POST",
            data: function (params) {
                return {
                    filter: params.term
                };
            },
            processResults: function (data) {
                var _d = [];
                _.each(data, function (item, i) {
                    _d.push({ id: item.ID, name: item.ClassName });
                })
                return {
                    results: _d
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: 1,
        templateResult: formatRepo2,
        templateSelection: formatRepoSelection2
    });



})