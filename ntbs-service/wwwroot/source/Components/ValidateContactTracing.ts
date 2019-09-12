import Vue from 'vue';
import { getHeaders, getValidationPath as getValidationPath, FormattedDate, convertFormattedDateToDate } from '../helpers';
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
    props: ['model', 'property', 'name'],
    methods: {
        validateAndCalculateTotals: function () {
            var contactTracingVariables: ContactTracingVariables = this.getContactTracingVariables();
            if (!contactTracingVariables) {
                return;
            }

            this.CalculateTotals(contactTracingVariables);

            this.ResetErrors();

            this.ValidateModel(contactTracingVariables);
            
        },
        CalculateTotals: function(contactTracingVariables: ContactTracingVariables) {
            this.$refs["totalContactsIdentified"].textContent = parseInt(contactTracingVariables.adultsIdentified) + parseInt(contactTracingVariables.childrenIdentified);
            this.$refs["totalContactsScreened"].textContent = parseInt(contactTracingVariables.adultsScreened) + parseInt(contactTracingVariables.childrenScreened);
            this.$refs["totalContactsActiveTB"].textContent = parseInt(contactTracingVariables.adultsActiveTB) + parseInt(contactTracingVariables.childrenActiveTB);
            this.$refs["totalContactsLatentTB"].textContent = parseInt(contactTracingVariables.adultsLatentTB) + parseInt(contactTracingVariables.childrenLatentTB);
            this.$refs["totalContactsStartedTreatment"].textContent = parseInt(contactTracingVariables.adultsStartedTreatment) + parseInt(contactTracingVariables.childrenStartedTreatment);
            this.$refs["totalContactsFinishedTreatment"].textContent = parseInt(contactTracingVariables.adultsFinishedTreatment) + parseInt(contactTracingVariables.childrenFinishedTreatment);
        },
        ResetErrors: function() {
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
        },

        ValidateModel: function(contactTracingVariables: ContactTracingVariables) {
            axios.get(`${this.$props.model}/Validate${this.$props.model}?key=${this.$props.property}&adultsIdentified=${contactTracingVariables.adultsIdentified}&childrenIdentified=${contactTracingVariables.childrenIdentified}&adultsScreened=${contactTracingVariables.adultsScreened}&childrenScreened=${contactTracingVariables.childrenScreened}&adultsLatentTB=${contactTracingVariables.adultsLatentTB}&childrenLatentTB=${contactTracingVariables.childrenLatentTB}&adultsActiveTB=${contactTracingVariables.adultsActiveTB}&childrenActiveTB=${contactTracingVariables.childrenActiveTB}&adultsStartedTreatment=${contactTracingVariables.adultsStartedTreatment}&childrenStartedTreatment=${contactTracingVariables.childrenStartedTreatment}&adultsFinishedTreatment=${contactTracingVariables.adultsFinishedTreatment}&childrenFinishedTreatment=${contactTracingVariables.childrenFinishedTreatment}`, 
                    null, { headers: getHeaders() })
                .then((response: any) => {
                    console.log(response);
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
                })
                .catch((error: any) => {
                    console.log(error.response)
                });
        },

        getContactTracingVariables: function() {

            var contactTracingVariables: ContactTracingVariables = {
                adultsIdentified: this.$refs["adultsIdentified"].value || 0,
                childrenIdentified: this.$refs["childrenIdentified"].value || 0,
                adultsScreened: this.$refs["adultsScreened"].value || 0,
                childrenScreened: this.$refs["childrenScreened"].value || 0,
                adultsLatentTB: this.$refs["adultsLatentTB"].value || 0, 
                childrenLatentTB: this.$refs["childrenLatentTB"].value || 0, 
                adultsActiveTB: this.$refs["adultsActiveTB"].value || 0,
                childrenActiveTB: this.$refs["childrenActiveTB"].value || 0,
                adultsStartedTreatment: this.$refs["adultsStartedTreatment"].value || 0, 
                childrenStartedTreatment: this.$refs["childrenStartedTreatment"].value || 0, 
                adultsFinishedTreatment: this.$refs["adultsFinishedTreatment"].value || 0,
                childrenFinishedTreatment: this.$refs["childrenFinishedTreatment"].value || 0
            }

            return contactTracingVariables;
        }
    }
});

export {
    ValidateContactTracing
};