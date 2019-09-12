import Vue from 'vue';
const axios = require('axios');

type ContactTracingVariables = { 
    adultsIdentified: any, 
    childrenIdentified: any, 
    adultsScreened: any,
    childrenScreened: any,
    adultsLatentTB: any, 
    childrenLatentTB: any, 
    adultsActiveTB: any,
    childrenActiveTB: any,
    adultsStartedTreatment: any, 
    childrenStartedTreatment: any, 
    adultsFinishedTreatment: any,
    childrenFinishedTreatment: any
};

const ValidateContactTracing = Vue.extend({
    props: ['model', 'property', 'name', 'rank'],
    data: function() {
        return {
            hasError: false
        }
    },
    mounted: function () {
        if (this.rank) {
            // v-model binds to the input event, so this gets picked up by the containing DateComparison component, if present
            // See https://vuejs.org/v2/guide/components.html#Using-v-model-on-Components
            var contactTracingVariables: ContactTracingVariables = this.getContactTracingVariables();
            if (contactTracingVariables) {
                this.$emit('input', contactTracingVariables);
            }
        }
    },
    methods: {
        validate: function () {
            var contactTracingVariables: ContactTracingVariables = this.getContactTracingVariables();
            if (!contactTracingVariables) {
                return;
            }

            this.$refs["totalContactsIdentified"].textContent = parseInt(contactTracingVariables.adultsIdentified) + parseInt(contactTracingVariables.childrenIdentified);
            this.$refs["totalContactsScreened"].textContent = parseInt(contactTracingVariables.adultsScreened) + parseInt(contactTracingVariables.childrenScreened);
            this.$refs["totalContactsActiveTB"].textContent = parseInt(contactTracingVariables.adultsActiveTB) + parseInt(contactTracingVariables.childrenActiveTB);
            this.$refs["totalContactsLatentTB"].textContent = parseInt(contactTracingVariables.adultsLatentTB) + parseInt(contactTracingVariables.childrenLatentTB);
            this.$refs["totalContactsStartedTreatment"].textContent = parseInt(contactTracingVariables.adultsStartedTreatment) + parseInt(contactTracingVariables.childrenStartedTreatment);
            this.$refs["totalContactsFinishedTreatment"].textContent = parseInt(contactTracingVariables.adultsFinishedTreatment) + parseInt(contactTracingVariables.childrenFinishedTreatment);

            //reset all error messages here?
            var listOfRefs: string[] = ["AdultsIdentified", "ChildrenIdentified", "AdultsScreened", "ChildrenScreened",
             "AdultsActiveTB", "ChildrenActiveTB", "AdultsLatentTB", "ChildrenLatentTB", "AdultsStartedTreatment",
             "ChildrenStartedTreatment", "AdultsFinishedTreatment", "ChildrenFinishedTreatment"];
            for(var i = 0; i < listOfRefs.length; i++) {
                let errorMessageRef = listOfRefs[i] + "ErrorRef";
                let formGroupRef = listOfRefs[i] + "FormGroup";
                let lowerCaseFirstLetterString = listOfRefs[i].charAt(0).toLowerCase() + listOfRefs[i].substring(1);
                this.$refs[errorMessageRef].textContent = "";
                this.$refs[lowerCaseFirstLetterString].classList.remove("nhsuk-input--error");
                this.$refs[formGroupRef].classList.remove("nhsuk-form-group--error");
            }

            var headers = {
                "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
            }

            axios.post(`${this.$props.model}/Validate${this.$props.model}?key=${this.$props.property}&adultsIdentified=${contactTracingVariables.adultsIdentified}&childrenIdentified=${contactTracingVariables.childrenIdentified}&adultsScreened=${contactTracingVariables.adultsScreened}&childrenScreened=${contactTracingVariables.childrenScreened}&adultsLatentTB=${contactTracingVariables.adultsLatentTB}&childrenLatentTB=${contactTracingVariables.childrenLatentTB}&adultsActiveTB=${contactTracingVariables.adultsActiveTB}&childrenActiveTB=${contactTracingVariables.childrenActiveTB}&adultsStartedTreatment=${contactTracingVariables.adultsStartedTreatment}&childrenStartedTreatment=${contactTracingVariables.childrenStartedTreatment}&adultsFinishedTreatment=${contactTracingVariables.adultsFinishedTreatment}&childrenFinishedTreatment=${contactTracingVariables.childrenFinishedTreatment}`, 
                    null, { headers: headers })
                .then((response: any) => {
                    var data = response.data;
                    for(let key in data) {
                        let value = data[key];
                        var errorRef = key + "ErrorRef";
                        var formGroupRef = key + "FormGroup";
                        let lowerCaseFirstLetterString = key.charAt(0).toLowerCase() + key.substring(1);
                        this.$refs[formGroupRef].classList.add("nhsuk-form-group--error");
                        this.$refs[lowerCaseFirstLetterString].classList.add("nhsuk-input--error");
                        this.$refs[errorRef].textContent = value;
                    }
                    if (!this.hasError && this.rank) {
                        this.$emit('input', contactTracingVariables);
                    }
                })
                .catch((error: any) => {
                    console.log(error.response)
                });
        },
        getContactTracingVariables: function() {
            const adultsIdentified = this.$refs["adultsIdentified"].value || 0;
            const childrenIdentified = this.$refs["childrenIdentified"].value || 0;
            const adultsScreened = this.$refs["adultsScreened"].value || 0;
            const childrenScreened = this.$refs["childrenScreened"].value || 0;
            const adultsLatentTB = this.$refs["adultsLatentTB"].value || 0;
            const childrenLatentTB = this.$refs["childrenLatentTB"].value || 0;
            const adultsActiveTB = this.$refs["adultsActiveTB"].value || 0;
            const childrenActiveTB = this.$refs["childrenActiveTB"].value || 0;
            const adultsStartedTreatment = this.$refs["adultsStartedTreatment"].value || 0;
            const childrenStartedTreatment = this.$refs["childrenStartedTreatment"].value || 0;
            const adultsFinishedTreatment = this.$refs["adultsFinishedTreatment"].value || 0;
            const childrenFinishedTreatment = this.$refs["childrenFinishedTreatment"].value || 0;

            var contactTracingVariables: ContactTracingVariables = {
                adultsIdentified: adultsIdentified,
                childrenIdentified: childrenIdentified,
                adultsScreened: adultsScreened,
                childrenScreened: childrenScreened,
                adultsLatentTB: adultsLatentTB, 
                childrenLatentTB: childrenLatentTB, 
                adultsActiveTB: adultsActiveTB,
                childrenActiveTB: childrenActiveTB,
                adultsStartedTreatment: adultsStartedTreatment, 
                childrenStartedTreatment: childrenStartedTreatment, 
                adultsFinishedTreatment: adultsFinishedTreatment,
                childrenFinishedTreatment: childrenFinishedTreatment
            }

            return contactTracingVariables;
        }
    }
});

export {
    ValidateContactTracing
};