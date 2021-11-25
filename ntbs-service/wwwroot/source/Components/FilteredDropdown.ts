import Vue from "vue";
import { getHeaders, buildPath } from "../helpers";
import axios from "axios";

type OptionValue = {
    value: string,
    text: string,
    group: string
};

const DEFAULT_SELECT_INNER_HTML = "<option value=\"\">Please select</option>";

/*
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
const FilteredDropdown = Vue.extend({
    props: {
        filterHandlerPath: {
            type: String
        },
        filteringRefs: {
            type: Array
        },
        waitForChildMount: {
            type: Boolean,
            default: true
        },
        hideOnEmpty: {
            type: Boolean,
            default: false
        }
    },
    mounted: function () {
        if (!this.waitForChildMount) {
            this.filteringMounted();
        }
    },
    methods: {
        filteringMounted: function () {
            this.fetchFilteredLists(this.getFilteringValue());
        },
        filteringChanged: function () {
            this.fetchFilteredLists(this.getFilteringValue());
        },
        getFilteringValue: function () {
            return this.getSelectElementInRef("filterContainer").value;
        },
        fetchFilteredLists: function (value: string) {
            const requestConfig = {
                url: buildPath(this.filterHandlerPath),
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

            if (this.hideOnEmpty) {
                this.hideOrShowDependentControl(refName, !values || values.length === 0);
            }
        },
        setSelectToValueOrDefault: function (selectElement: HTMLSelectElement, targetValue: string) {
            for (let i = 0; i < selectElement.options.length; i++) {
                if (selectElement.options[i].value === targetValue) {
                    selectElement.value = targetValue;
                    return;
                }
            }

            selectElement.value = "";
            selectElement.dispatchEvent(new Event("change"));
        },
        getSelectElementInRef(refName: string) {
            const filterValueContainer = this.$refs[refName];
            if (filterValueContainer.$refs) {
                const selectField = this.findSelectFieldInContainer(filterValueContainer);
                if (selectField) {
                    return selectField;
                }
            }

            return filterValueContainer.getElementsByTagName("select")[0];
        },
        hideOrShowDependentControl: function (refName: string, isValuesEmpty: boolean) {
            const ref = this.$refs[refName];
            let controlContainer: any;

            if (ref instanceof Vue) {
                controlContainer = ref.$el;
            } else {
                controlContainer = ref;
            }

            if (isValuesEmpty) {
                controlContainer.classList.add("hidden");
            } else {
                controlContainer.classList.remove("hidden");
            }
        },
        generateOptionInnerHtml(values: OptionValue[]) {
            if (values.some(entry => entry.group)) {
                return this.generateGroupedOptionsInnerHtml(values);
            }

            let optionInnerHtml = DEFAULT_SELECT_INNER_HTML;
            values.forEach(item => optionInnerHtml += `<option value="${item.value}">${item.text}</option>`);
            return optionInnerHtml;
        },
        generateGroupedOptionsInnerHtml(values: OptionValue[]) {
            let optionInnerHtml = DEFAULT_SELECT_INNER_HTML;

            values.sort((a, b) => {
                if (a.group > b.group) { return -1 }
                if (a.group < b.group) { return 1 }
                return 0;
            });

            let currentGroup = values[0].group;
            optionInnerHtml += `<optgroup label="${currentGroup}">`;

            values.forEach(item => {
                if (item.group !== currentGroup) {
                    optionInnerHtml += "</optgroup>";
                    currentGroup = item.group;
                    optionInnerHtml += `<optgroup label="${currentGroup}">`;
                }

                optionInnerHtml += `<option value="${item.value}">${item.text}</option>`;
            });

            optionInnerHtml += "</optgroup>";
            return optionInnerHtml;
        },
        findSelectFieldInContainer(container: Vue) {
            if (container.$refs["selectField"]) {
                return container.$refs["selectField"];
            }
            let selectField: Vue;
            for(let i = 0; i < container.$children.length; i++) {
                selectField = this.findSelectFieldInContainer(container.$children[i]);
                if (selectField) {
                    return selectField;
                }
            }
            return;
        }
    }
});

export default FilteredDropdown;
