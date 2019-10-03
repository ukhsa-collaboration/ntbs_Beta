import Vue from 'vue';
import { getHeaders } from '../helpers';
import axios from 'axios';

type ContactTracingVariables = { 
    adultsIdentified: string,
    childrenIdentified: string,
    adultsScreened: string,
    childrenScreened: string,
    adultsLatentTB: string,
    childrenLatentTB: string,
    adultsActiveTB: string,
    childrenActiveTB: string,
    adultsStartedTreatment: string,
    childrenStartedTreatment: string,
    adultsFinishedTreatment: string,
    childrenFinishedTreatment: string
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
            let requestConfig = {
                url: `${this.$props.model}/Validate${this.$props.model}?key=${this.$props.property}`,
                headers: getHeaders(),
                params: contactTracingVariables
              }

            axios.request(requestConfig)
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