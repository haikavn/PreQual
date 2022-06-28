
var FbCurrentPage=1;
function FormBuilder()
{
    const mainContext = this;
    this.Name = "Form";
    this.Pages = new Array();
    this.Container = "formcontainer";
    this.IsWizard = true;    

    this.onSubmitTemplate = `
        <div style="display: none" id="finalPage" class="container mt30">
            <div class="text-center blackColor">
                <div class="row fontSize30 ">Searching finished</div>
                <div class="row mt30 fontSize18">Sorry, but you were not matched with any lenders</div>
                <div class="row imgSize mt30 finalPage-image">
                    <\img src="img/decline-page-pic.png" />
                </div>
            </div>
        </div>
    `;

    this.onSubmitLoader = `
        <div id="loader" class="text-center mt30">
            <div class="blackColor fontSize30">Stay tuned!</div>
            <div class="mt30 blackColor fontSize18">We are trying our best to find you a match, will be right back soon!</div>
            <div class="mt30 fontSize18">Please do not refresh the page.</div>
            <img class="loadingImg" src="img/ezgif.com-gif-maker (18).gif" />
        </div>
    `;

    this.showOnSubmitPopup = function() {
        const form = document.getElementById('form');
        const container = document.getElementById('formcontainer');
        form.style.display = 'none';
        container.insertAdjacentHTML('afterBegin', this.onSubmitLoader);
        container.insertAdjacentHTML('afterBegin', this.onSubmitTemplate);
    };
    this.ParseSource = function (str)
    {
        var data = str;
        if (typeof str=="string")
            data = JSON.parse(str);
        //if (data.IsWizard !== undefined)
          //  this.IsWizard = data.IsWizard;

        var pages = new Object();
        var pageProp = new Object();

        if (data.PageProperties)
        for (var i = data.PageProperties.length - 1; i >= 0; i--)
        {
            var step = data.PageProperties[i].Step;
            if (step == undefined)
                step = i;
            pageProp[step] = data.PageProperties[i];
        }


        for (var i=data.Fields.length-1; i>=0; i--) {
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

            if (!pageProp[key]) {
                pages[key].Properties = new Object();
                pages[key].Properties.Title = "Step " + this.Pages.length;
            }
            else {
                pages[key].Properties = pageProp[key];
            }
        }

        this.Pages.sort(function (a, b) {
            return a.Step - b.Step;
        });


        this.data = data;
    };


    this.template = `<div class="container @FormClass" style="padding-top:10px; bacgkround: @FormBackground; width:@FormWidth; height:@FormHeight; @FormStyle">
                    <form id="form">
                      <!--<ul id="progressbar">
                        @progressbar
                    </ul>-->
                  
                    @pages
                    
                </form>
            </div>`;


    this.IPAddress="";
    this.Init = function () {
        var gen = this.Generate();
        $("#" + this.Container).html(gen);
        this.InitScripts();
	var scope=this;    
        $.getJSON('https://ipinfo.io/json?token=eab2e0d1f63ab2', function(data) {
		scope.IPAddress=data.ip;	  
	});
    };


    

    this.GetXML = function () {

        var out = "<REQUEST>\n";
        var referral = `
        <REFERRAL>
        <CHANNELID>@channel</CHANNELID>
        <PASSWORD>@password</PASSWORD>
        @affsubid        
        <REFERRINGURL>@refurl</REFERRINGURL>
        <MINPRICE>@minprice</MINPRICE>
        </REFERRAL>\n`;

        SubmitReturnURL = this.data.SubmitReturnURL;
        if (!SubmitReturnURL)
            SubmitReturnURL = "/";

        referral = referral.replace("@channel", this.data.ChannelId);
        referral = referral.replace("@password", this.data.Password);
        referral = referral.replace("@refurl", this.data.ReferringUrl);
        referral = referral.replace("@minprice", this.data.MinPrice);
        
        var affSubIds = this.data.AffSubIds;
        var aff = "";
        if (affSubIds) {
            for (var i = 0; i < affSubIds.length; i++) {
                aff += "<"+affSubIds[i].Name+">";
                aff += affSubIds[i].Value;
                aff += "</"+affSubIds[i].Name+">";
            }
        }

        referral=referral.replace("@affsubid", aff);


        out += referral;

        out += "<CUSTOMER>\n";
        var sections = new Object();
        for (var i = 0; i < this.Pages.length; i++)
        {
            var xmlSectionName = this.Pages[i].Properties.XMLSectionName;
            if (!xmlSectionName)
                xmlSectionName = "PERSONAL";

            if (sections[xmlSectionName] == undefined)
                sections[xmlSectionName] = "";

            var page = this.Pages[i];
	    	
            var xml = "";
		if (xmlSectionName=="PERSONAL") 
		{
			xml=page.GetPageXML(this.IPAddress);
			this.IPAddress="";
		}
		else
			xml=page.GetPageXML();

            sections[xmlSectionName] += xml+"\n";
        };

        
        for (var key in sections)
        {
            out += "<" + key + ">" + sections[key] + "</" + key + ">";
        }

        out += "</CUSTOMER>\n";
        out += "</REQUEST>";

        return out;
    };

    this.Generate = function () {
        var li = "";
        var pages = "";
        for (var i = 0; i < this.Pages.length; i++) {
            li += ` <li class="active" id="step` + (i + 1).toString() + `">
                            <strong></strong>
                        </li>`;

	
            var page=this.Pages[i].Generate("Page"+i);
	
	    if (this.data.ShowNextPrevButton===false || this.Pages[i].Properties.ShowNextPrevButton===false)
		{
			page=page.replaceAll("@NextStepDisplay","display:none");
		}

		pages += page;

        }

        var gen = this.template;
        gen = gen.replace("@progressbar", li);

        for (var key in this.data) {
            gen = gen.replace("@"+key, this.data[key]);
        }
        gen = gen.replace("@pages", pages);

        
        return gen;
    };

    this.InitScripts=function()
    {
        
            var currentGfgStep, nextGfgStep, previousGfgStep;
            var opacity;
            var current = 1;
	    FbCurrentPage=1;
            var steps = $("fieldset").length;

        setProgressBar(current);

        $(document).on("keyup", function (event) {
            // Number 13 is the "Enter" key on the keyboard
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                $(".next-step:visible").trigger("click");
            }
        });


	var scope = this;

        $($(".previous-step")[0]).hide();

        

            $(".next-step").click(function () {
                let stepInputs = $('#form .formFieldset').eq(current-1).find('input:not([type="hidden"])')
                const errors = []
                stepInputs.each((index, stepInput) => {
                    if (stepInput.getAttribute("type") === 'button') {
                        return
                    }
                    if (!(new RegExp(stepInput.getAttribute("pattern"))).test(stepInput.value)) {
                        const label = stepInput.previousElementSibling ? stepInput.previousElementSibling.innerText : "Unknown input"
                        errors.push(`Your ${label} is not valid`)
                    }
                    if (!stepInput.value) {
                        const label = stepInput.previousElementSibling ? stepInput.previousElementSibling.innerText : "Unknown input"
                        errors.push(`Your ${label} is empty`)
                    }
                })
                if (errors.length) {
                    const oldScreen = document.getElementById("errorScreen")
                    if (oldScreen) {
                        oldScreen.parentElement.removeChild(oldScreen);
                    }
                    const containerElement = document.createElement("div")
                    containerElement.setAttribute("id", "errorScreen")
                    const listElement = document.createElement("div")
                    listElement.classList.add('err-list')
                    containerElement.appendChild(listElement)
                    errors.forEach(errorText => {
                        const listItemElement = document.createElement("span")
                        listElement.appendChild(listItemElement)
                        const errorMessage = document.createTextNode(errorText)
                        listItemElement.appendChild(errorMessage)
                    })
                    containerElement.classList.add('visible');
                    const root = document.getElementById('form');
                    root.appendChild(containerElement);
                    /*setTimeout(() => {
                        const oldScreen = document.getElementById("errorScreen")
                        if (oldScreen) {
                            oldScreen.parentElement.removeChild(oldScreen);
                        }
                    }, 3000)*/
                    return
                }
                if (current == steps) {
                    const _this = mainContext;
                    _this.showOnSubmitPopup();

                    var xml = scope.GetXML();
                    AdrackFormBuilderPostXML(null, xml);
                    return;
                }
	
                currentGfgStep = $(this).parent();
                nextGfgStep = $(this).parent().next();

                $("#progressbar li").eq($("fieldset")
                    .index(nextGfgStep)).addClass("active");

                nextGfgStep.show();
		//currentGfgStep.hide();

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
		FbCurrentPage=current;
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
		FbCurrentPage=current;
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

            //var dd=$('.selectpickerm');
	    //dd.selectpicker();
        
    }
}

//