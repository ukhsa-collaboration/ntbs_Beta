import Vue from "vue";
import {convertStringToNullableInt, getHeaders} from "../helpers";
import axios, {Method} from "axios";

type ContactTracingVariables = {
    adultsIdentified: number,
    childrenIdentified: number,
    adultsScreened: number,
    childrenScreened: number,
    adultsLatentTb: number,
    childrenLatentTb: number,
    adultsActiveTb: number,
    childrenActiveTb: number,
    adultsStartedTreatment: number,
    childrenStartedTreatment: number,
    adultsFinishedTreatment: number,
    childrenFinishedTreatment: number
};

const ValidateContactTracing = Vue.extend({
    props: ["model", "property"],
    methods: {
        validateAndCalculateTotals: function () {
            const contactTracingVariables: ContactTracingVariables = this.getContactTracingVariables();
            if (!contactTracingVariables) {
                return;
            }

            this.CalculateTotals(contactTracingVariables);

            this.ResetErrors();

            this.ValidateModel(contactTracingVariables);

        },
        CalculateTotals: function (contactTracingVariables: ContactTracingVariables) {
            this.$refs["totalContactsIdentified"].textContent = contactTracingVariables.adultsIdentified + contactTracingVariables.childrenIdentified;
            this.$refs["totalContactsScreened"].textContent = contactTracingVariables.adultsScreened + contactTracingVariables.childrenScreened;
            this.$refs["totalContactsActiveTB"].textContent = contactTracingVariables.adultsActiveTb + contactTracingVariables.childrenActiveTb;
            this.$refs["totalContactsLatentTB"].textContent = contactTracingVariables.adultsLatentTb + contactTracingVariables.childrenLatentTb;
            this.$refs["totalContactsStartedTreatment"].textContent = contactTracingVariables.adultsStartedTreatment + contactTracingVariables.childrenStartedTreatment;
            this.$refs["totalContactsFinishedTreatment"].textContent = contactTracingVariables.adultsFinishedTreatment + contactTracingVariables.childrenFinishedTreatment;
        },
        ResetErrors: function () {
            const listOfRefs: string[] = ["AdultsIdentified", "ChildrenIdentified", "AdultsScreened", "ChildrenScreened",
                "AdultsActiveTB", "ChildrenActiveTB", "AdultsLatentTB", "ChildrenLatentTB", "AdultsStartedTreatment",
                "ChildrenStartedTreatment", "AdultsFinishedTreatment", "ChildrenFinishedTreatment"];
            for (let i = 0; i < listOfRefs.length; i++) {
                const errorMessageRef = listOfRefs[i] + "ErrorRef";
                const formGroupRef = listOfRefs[i] + "FormGroup";
                const lowerCaseFirstLetterString = listOfRefs[i].charAt(0).toLowerCase() + listOfRefs[i].substring(1);
                this.$refs[errorMessageRef].textContent = "";
                this.$refs[errorMessageRef].classList.add("hidden");
                this.$refs[lowerCaseFirstLetterString].classList.remove("nhsuk-input--error");
                this.$refs[formGroupRef].classList.remove("nhsuk-form-group--error");
            }
        },

        ValidateModel: function (contactTracingVariables: ContactTracingVariables) {
            let requestConfig = {
                method: "post" as Method,
                url: `${this.$props.model}/Validate${this.$props.model}`,
                headers: getHeaders(),
                data: contactTracingVariables
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    var data = response.data;
                    for (let key in data) {
                        if (data.hasOwnProperty(key)) {
                            // Model errors can be double counted as 'Model.Property' and 'Property'.
                            // Here it's convenient to act on 'Property' only
                            if (key.indexOf(".") === -1) {
                                const value = data[key];
                                const errorRef = key + "ErrorRef";
                                const formGroupRef = key + "FormGroup";
                                const lowerCaseFirstLetterString = key.charAt(0).toLowerCase() + key.substring(1);
                                this.$refs[formGroupRef].classList.add("nhsuk-form-group--error");
                                this.$refs[lowerCaseFirstLetterString].classList.add("nhsuk-input--error");
                                this.$refs[errorRef].classList.remove("hidden");
                                this.$refs[errorRef].textContent = value;
                            }
                        }
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        },

        getContactTracingVariables: function () {
            const contactTracingVariables: ContactTracingVariables = {
                adultsIdentified: convertStringToNullableInt(this.$refs["adultsIdentified"].value),
                childrenIdentified: convertStringToNullableInt(this.$refs["childrenIdentified"].value),
                adultsScreened: convertStringToNullableInt(this.$refs["adultsScreened"].value),
                childrenScreened: convertStringToNullableInt(this.$refs["childrenScreened"].value),
                adultsLatentTb: convertStringToNullableInt(this.$refs["adultsLatentTB"].value),
                childrenLatentTb: convertStringToNullableInt(this.$refs["childrenLatentTB"].value),
                adultsActiveTb: convertStringToNullableInt(this.$refs["adultsActiveTB"].value),
                childrenActiveTb: convertStringToNullableInt(this.$refs["childrenActiveTB"].value),
                adultsStartedTreatment: convertStringToNullableInt(this.$refs["adultsStartedTreatment"].value),
                childrenStartedTreatment: convertStringToNullableInt(this.$refs["childrenStartedTreatment"].value),
                adultsFinishedTreatment: convertStringToNullableInt(this.$refs["adultsFinishedTreatment"].value),
                childrenFinishedTreatment: convertStringToNullableInt(this.$refs["childrenFinishedTreatment"].value)
            };

            return contactTracingVariables;
        }
    }
});

export default ValidateContactTracing;