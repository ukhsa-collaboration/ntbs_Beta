import Vue from "vue";

const accessible = require("accessible-autocomplete");

const AutocompleteSelect = Vue.extend({
    props: ["placeholder", "emitChangeOn"],
    mounted: function() {
        this.selectElement = this.$refs["selectField"];
        const selectId = this.selectElement.id;
        const options: any = {
            placeholder: this.$props.placeholder,
            autoselect: false,
            showAllValues: true,
            selectElement: this.selectElement
        };
        if (this.$props.emitChangeOn) {
            options.onConfirm = (val: any) => {
                this.onConfirm(val);
            }
        };
        accessible.enhanceSelectElement(options);
        this.autocompleteElement = document.getElementById(selectId) as HTMLInputElement;
        // Workaround for stopping Chrome trying to autocomplete by country, see discussion https://gist.github.com/niksumeiko/360164708c3b326bd1c8
        // Note that autocomplete="off" does not work.
        this.autocompleteElement.setAttribute('autocomplete', 'no');
        // In IE, name gets automatically populated with empty string and razor then tries to bind this value to the model, so we need to remove name completely
        this.autocompleteElement.removeAttribute('name');
        if (this.$props.emitChangeOn) {
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
        onConfirm: function (value: any) {
            // this is called twice, when a user selects a value (value is defined) and when user clicks away from input (blur, value undefined)
            if (!this.$props.emitChangeOn) {
                return;
            }
            if (this.$props.emitChangeOn === "blur" && value === undefined) {
                this.setSelectValue(this.autocompleteElement.value);
            }
            else if (this.$props.emitChangeOn === "select" && value !== undefined) {
                this.setSelectValue(value);
            }
        },
        setSelectValue: function (value: any) {
            const event = new Event('select-changed');
            // the actual select element does not have its value set at this point; the only way we seem to be able to find it is by searching for the matching autocomplete
            // value by name in all the options
            for (let i=0; i < this.selectElement.options.length; i++) {
                if (value === this.selectElement[i].textContent) {
                    const previousValue = this.selectElement.value;
                    this.selectElement.value = this.selectElement[i].value;
                    if (this.selectElement.value != previousValue) {
                        this.selectElement.dispatchEvent(event);
                    }
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