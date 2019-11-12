import Vue from "vue";
var accessible = require("accessible-autocomplete");

const AutocompleteSelect = Vue.extend({
    props: ["placeholder", "validate"],
    mounted: function() {
        this.selectElement = this.$refs["selectElement"];
        var selectId = this.selectElement.id;
        accessible.enhanceSelectElement({
            placeholder: this.$props.placeholder,
            autoselect: false,
            preserveNullOptions: true,
            showAllValues: true,
            selectElement: this.selectElement,
            onConfirm: (val: any) => {
                this.onConfirm(val);
            }
        })
        this.autocompleteElement = document.getElementById(selectId) as HTMLInputElement;
        // Workaround for stopping Chrome trying to autocomplete by country, see discussion https://gist.github.com/niksumeiko/360164708c3b326bd1c8
        // Note that autocomplete="off" does not work.
        this.autocompleteElement.setAttribute('autocomplete', 'no');
        if (this.$props.validate) {
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
                this.$emit("validate-input", event);
            }
        },
        onConfirm: function (value: any) {
            // this is called twice, value is undefined when user clicks away from input field which is when we want validation to happen
            if (!this.$props.validate || value != undefined) {
                return;
            }
            var event = new Event('select-changed');
            var autocompleteValue = this.autocompleteElement.value;
            // the actual select element does not have its value set at this point; the only way we seem to be able to find it is by searching for the matching autocomplete
            // value by name in all the options
            for (var i=0; i < this.selectElement.options.length; i++) {
                if (autocompleteValue === this.selectElement[i].textContent) {
                    this.selectElement.value = this.selectElement[i].value;
                    this.selectElement.dispatchEvent(event);
                    break;
                }
            }
        },
        selectChanged: function (event: Event) {
            // to ensure containing validation method called
            this.$emit("validate-input", event);
        }
    }
});

export {
    AutocompleteSelect
};