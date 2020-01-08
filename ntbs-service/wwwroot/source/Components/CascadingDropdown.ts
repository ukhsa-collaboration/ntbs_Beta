import Vue from "vue";
import { getHeaders } from "../helpers";
import axios from "axios";

type OptionValue = {
    value: string,
    text: string,
    group: string
};

const DEFAULT_SELECT_INNER_HTML = "<option value=\"\">Please select</option>";

/*
 Triple filtering dropdown
 The component only works for a single select field to filter one or more other fields, 
 any cascading nonsense will require more specific handling.

 'filteringRefs' prop is required to be an array of strings corresponding to a 
 ref containing a select element (if ref is a vue component, requires a 'selectField' ref within that).
 Each string in the array is also an expected response-object key from the endpoint:
 see OnGetGetFilteredListsByTbService in Episode.cshtml.cs

 Additionally to the ref requirements above the input to drive the filtering must be within a ref
 of name 'filterContainer' containing a select element (if ref is a vue component, requires a 'selectField' ref within that).

 waitForChildMount is intended to be used to either trigger filtering on mount of this component,
 or if relevant to be triggered on mount of the filterContainer's vue component.

 hideOnEmptyOptions configures whether the component should hide the dependent control if the returned list 
 of OptionValues from the server is empty.
*/
const CascadingDropdown = Vue.extend({
    props: {
        filterFirstHandlerPath: {
            type: String
        },
        filterSecondHandlerPath: {
            type: String
        },
        filteringRefs: {
            type: Array
        },
        waitForChildMount: {
            type: Boolean,
            default: true
        }
    },
    mounted: function () {
        if (!this.waitForChildMount) {
            this.filteringMounted();
        }
    },
    methods: {
        filteringMounted: function () {
            this.fetchFirstFilteredList(this.getFirstFilteringValue(), this.getSecondFilteringValue());
        },
        firstFilteringChanged: function () {
            this.fetchFirstFilteredList(this.getFirstFilteringValue());
        },
        secondFilteringChanged: function () {
            this.fetchSecondFilteredList(this.getSecondFilteringValue());
        },
        getFirstFilteringValue: function () {
            return this.getSelectElementInRef("firstFilterContainer").value;
        },
        getSecondFilteringValue: function () {
            return this.getSelectElementInRef("tbServices").value;
        },
        fetchFirstFilteredList: function (value: string) {
            const requestConfig = {
                url: `${window.location.pathname}/${this.filterFirstHandlerPath}`,
                headers: getHeaders(),
                params: {
                    "value": value
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    for (let key in response.data) {
                        if (response.data.hasOwnProperty(key)) {
                            if (this.filteringRefs.indexOf(key) !== -1) {
                                this.updateSelectList(key, response.data[key]);
                            }
                        }
                    }
                });
        },
        fetchSecondFilteredList: function (value: string) {
            const requestConfig = {
                url: `${window.location.pathname}/${this.filterSecondHandlerPath}`,
                headers: getHeaders(),
                params: {
                    "value": value
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    for (let key in response.data) {
                        if (response.data.hasOwnProperty(key)) {
                            if (this.filteringRefs.indexOf(key) !== -1) {
                                this.updateSelectList(key, response.data[key]);
                            }
                        }
                    }
                });
        },
        updateSelectList: function (refName: string, values: OptionValue[]) {
            const selectElement = this.getSelectElementInRef(refName);
            let currentSelectedValue = selectElement.value;

            if (values) {
                const optionInnerHtml = this.generateOptionInnerHtml(values);
                selectElement.innerHTML = optionInnerHtml;
            } else {
                currentSelectedValue = "";
            }

            this.setSelectToValueOrDefault(selectElement, currentSelectedValue);
        },
        setSelectToValueOrDefault: function (selectElement: HTMLSelectElement, targetValue: string) {
            for (let i = 0; i < selectElement.options.length; i++) {
                if (selectElement.options[i].value === targetValue) {
                    selectElement.value = targetValue;
                    return;
                }
            }

            selectElement.value = "";
        },
        getSelectElementInRef(refName: string) {
            const filterValueContainer = this.$refs[refName];
            if (filterValueContainer.$refs) {
                return filterValueContainer.$refs["selectField"];
            }

            return filterValueContainer.getElementsByTagName("select")[0];
        },
        generateOptionInnerHtml(values: OptionValue[]) {
            if (values.some(entry => entry.group)) {
                return this.generateGroupedOptionsInnerHtml(values);
            }

            let optionInnerHtml = DEFAULT_SELECT_INNER_HTML;
            values.forEach(item => optionInnerHtml += `<option value="${item.value}">${item.text}</option>`);
            return optionInnerHtml;
        },
    }
});

export default CascadingDropdown;
