import Vue from "vue";

const accessible = require("accessible-autocomplete");

const AutocompleteSelect = Vue.extend({
    props: ["placeholder", "validate", "filter"],
    mounted: function() {
        this.selectElement = this.$refs["selectField"];
        const selectId = this.selectElement.id;
        const options: any = {
            placeholder: this.$props.placeholder,
            autoselect: false,
            showAllValues: true,
            selectElement: this.selectElement
        };
        if (this.$props.validate) {
            options.onConfirm = (val: any) => {
                this.onConfirmValidate(val);
            }
        };
        if (this.$props.filter) {
            options.onConfirm = (val: any) => {
                this.onConfirmFilter(val);
            }
        };
        accessible.enhanceSelectElement(options);
        this.autocompleteElement = document.getElementById(selectId) as HTMLInputElement;
        // Workaround for stopping Chrome trying to autocomplete by country, see discussion https://gist.github.com/niksumeiko/360164708c3b326bd1c8
        // Note that autocomplete="off" does not work.
        this.autocompleteElement.setAttribute('autocomplete', 'no');
        // In IE, name gets automatically populated with empty string and razor then tries to bind this value to the model, so we need to remove name completely
        this.autocompleteElement.removeAttribute('name');
        if (this.$props.validate || this.$props.filter) {
            // As we do not build the element ourselves, use ordinary event listeners rather than vue's
            this.autocompleteElement.addEventListener('change', (event: any) => this.inputChanged(event));
            this.selectElement.addEventListener('select-changed', (event: any) => this.selectChanged(event));
        }
    },
    methods: {
        inputChanged: function (event: FocusEvent) {
            if (this.autocompleteElement.value === "")
            {
                // Autocomplete does not automatically select blank value when deleting text rather than clicking on blank, so emit this manually
                this.selectElement.value = "";
                this.$emit("input-changed", event);
            }
        },
        onConfirmValidate: function (value: any) {
            // this is called twice, value is undefined when user clicks away from input field which is when we want validation to happen
            if (!this.$props.validate || value != undefined) {
                return;
            }
            this.setSelectValue(this.autocompleteElement.value);
        },
        onConfirmFilter: function (value: any) {
            // this is called twice, value is defined when user makes a selection which is when we want filtering to happen
            if (!this.$props.filter) {
                return;
            }
            if (value != undefined) {
                this.setSelectValue(value);
            }
        },
        setSelectValue: function (value: any) {
            const event = new Event('select-changed');
            // the actual select element does not have its value set at this point; the only way we seem to be able to find it is by searching for the matching autocomplete
            // value by name in all the options
            for (let i=0; i < this.selectElement.options.length; i++) {
                if (value === this.selectElement[i].textContent) {
                    this.selectElement.value = this.selectElement[i].value;
                    this.selectElement.dispatchEvent(event);
                    break;
                }
            };
        },
        selectChanged: function (event: Event) {
            // to ensure containing validation method called
            this.$emit("input-changed", event);
        }
    }
});

export default AutocompleteSelect;