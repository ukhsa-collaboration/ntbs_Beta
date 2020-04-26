import Vue from "vue";

export default Vue.extend({
    mounted: function() {
        this.$refs["dayInput"].addEventListener('input', this.moveFocusIfFull(this.$refs["monthInput"]));
        this.$refs["monthInput"].addEventListener('input', this.moveFocusIfFull(this.$refs["yearInput"]));
    },
    methods: {
        moveFocusIfFull: (nextInput: HTMLInputElement) => function(ev: InputEvent) {
            const currnetInput = <HTMLInputElement> ev.target;
            if (currnetInput.value.length == 2) {
                nextInput.focus();
            }
        }
    }
})