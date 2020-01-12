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
 Triple filtering dropdown based off FilteringDropdown.ts
 
 This is used in TransferRequest.cshtml currently where the first dropdown filters the next (and resets the 3rd dropdown), and the second dropdown
 filters the third dropdown.

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
        hiddenFirstDropdownWithNoJs: {
            type: Boolean
        }
    },
    mounted: function () {
        this.filteringMounted();
    },
    methods: {
        filteringMounted: function () {
            this.fetchFilteredList("", this.filterSecondHandlerPath);
            if(this.hiddenFirstDropdownWithNoJs)
            {
                this.$refs["firstFilterContainer"].classList.remove("hidden");
            }
        },
        firstFilteringChanged: function () {
            this.fetchFilteredList(this.getFirstFilteringValue(), this.filterFirstHandlerPath);
        },
        secondFilteringChanged: function () {
            this.fetchFilteredList(this.getSecondFilteringValue(), this.filterSecondHandlerPath);
        },
        getFirstFilteringValue: function () {
            return this.getSelectElementInRef("firstFilterContainer").value;
        },
        getSecondFilteringValue: function () {
            return this.getSelectElementInRef(this.filteringRefs[0]).value;
        },
        fetchFilteredList: function (value: string, path: string) {
            const requestConfig = {
                url: `${window.location.pathname}/${path}`,
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

            if (values) {
                const optionInnerHtml = this.generateOptionInnerHtml(values);
                selectElement.innerHTML = optionInnerHtml;
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
