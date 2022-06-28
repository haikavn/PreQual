function FormBuilder()
{
    this.Name = "Form";
    this.Pages = new Array();
    this.Container = "formcontainer";
    this.IsWizard = true;    

    this.ParseSource = function (str)
    {
        
        var data = str;
        if (typeof str=="string")
            data = JSON.parse(str);
        //if (data.IsWizard !== undefined)
          //  this.IsWizard = data.IsWizard;

        var pages = new Object();
        var pageTitles = data.PageTitles;
        for (var i = 0; i < data.Fields.length; i++) {
            var field = new FormField();
            field.Style = data.Fields[i].Style;
            field.Properties = data.Fields[i].Properties;
            field.Layout = data.Fields[i].Layout;
            field.Name = data.Fields[i].Name;
            field.Step = data.Fields[i].Step;
            if (!this.IsWizard)
                field.Step = 0;
            
            if (pages[field.Step] == undefined) {
                pages[field.Step] = new FormPage();
            }
            
            pages[field.Step].Fields.push(field);
            pages[field.Step].Step = field.Step;
        };

        for (var key in pages) {
            
            this.Pages.push(pages[key]);

            if (!data.PageTitles)
                pages[key].Name = "Step " + this.Pages.length;
            else
            pages[key].Name = data.PageTitles[this.Pages.length - 1];
        }

        this.Pages.sort(function (a, b) {
            return a.Step - b.Step;
        });
    };


    this.template = `<div class="container rightgeneralsection" style="padding-top:10px;">
                    <form id="form">
                      <ul id="progressbar">
                        @progressbar
                    </ul>
                    <div class="progress">
                        <div class="progress-bar"></div>
                    </div>
                    @pages
                    
                </form>
            </div>`;


    this.Init = function () {
        var gen = this.Generate();
        $("#" + this.Container).html(gen);
        this.InitScripts();
    };

    this.Generate = function () {
        var li = "";
        var pages = "";
        for (var i = 0; i < this.Pages.length; i++) {
            li += ` <li class="active" id="step` + (i + 1).toString() + `">
                            <strong></strong>
                        </li>`;
            pages += this.Pages[i].Generate();
        }

        var gen = this.template;
        gen = gen.replace("@progressbar", li);
        gen = gen.replace("@pages", pages);
        return gen;
    };

    this.InitScripts=function()
    {
        
            var currentGfgStep, nextGfgStep, previousGfgStep;
            var opacity;
            var current = 1;
            var steps = $("fieldset").length;

            setProgressBar(current);

            $($(".previous-step")[0]).hide();

            $(".next-step").click(function () {
                if (current == steps) {
                    alert('Submit');
                    return;
                }
                currentGfgStep = $(this).parent();
                nextGfgStep = $(this).parent().next();

                $("#progressbar li").eq($("fieldset")
                    .index(nextGfgStep)).addClass("active");

                nextGfgStep.show();
                currentGfgStep.animate({ opacity: 0 }, {
                    step: function (now) {
                        opacity = 1 - now;

                        currentGfgStep.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        nextGfgStep.css({ 'opacity': opacity });
                    },
                    duration: 500
                });
                setProgressBar(++current);
                if (current == steps) {
                    $($(".next-step")[steps - 1]).val("Submit");
                }
            });

            $(".previous-step").click(function () {

                currentGfgStep = $(this).parent();
                previousGfgStep = $(this).parent().prev();

                $("#progressbar li").eq($("fieldset")
                    .index(currentGfgStep)).removeClass("active");

                previousGfgStep.show();

                currentGfgStep.animate({ opacity: 0 }, {
                    step: function (now) {
                        opacity = 1 - now;

                        currentGfgStep.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        previousGfgStep.css({ 'opacity': opacity });
                    },
                    duration: 500
                });
                setProgressBar(--current);
            });

            function setProgressBar(currentStep) {
                var percent = parseFloat(100 / steps) * current;
                percent = percent.toFixed();
                $(".progress-bar")
                    .css("width", percent + "%")
            }

            $(".submit").click(function () {
                return false;
            })

            var dd=$('.selectpickerm');
	    dd.selectpicker();
        
    }
}

