import Vue from "vue";

const CaseManagerMenuDropdown = Vue.extend({
    created(): void {
        window.onclick = this.hideNavDropdown.bind(this);
    },
    methods: {
        hideNavDropdown: function (event: MouseEvent )
        {
            let inputElement = (<HTMLInputElement>event.target);
            if (!inputElement.classList.contains('dropdown-header')) {
                let dropdowns = document.getElementsByClassName("nav-menu-with-dropdown-list");
                for (let i = 0; i < dropdowns.length; i++) {
                    let openDropdown = dropdowns[i];
                    if (openDropdown.classList.contains('show')) {
                        openDropdown.classList.remove('show');
                    }
                }
            }
        },
        toggleMenu: function () {
            this.$refs["case-manager-submenu"].classList.toggle("show");
        }

    }
});

export default CaseManagerMenuDropdown;