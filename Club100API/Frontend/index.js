// use https (http secure).
// http (non-secure) will make the app complain about mixed content when running the app from Azure
const baseUrl = "https://localhost:7067/api/members"

Vue.createApp({
    data() {
        return {
            // ============================================================
            // All members (used for client-side filter reset)
            // Teacher File 2 pattern: allBooks + books
            // ============================================================
            allMembers: [],
            members: [],

            // ============================================================
            // Get by name and ID
            // ============================================================
            nameToGetBy: "",
            idToGetBy: null,
            singleMember: null,

            // ============================================================
            // Delete
            // ============================================================
            deleteId: null,
            deleteMessage: "",

            // ============================================================
            // Add
            // ============================================================
            addData: { name: "", club: "", count: null },
            addMessage: "",

            // ============================================================
            // Update
            // ============================================================
            updateData: { id: null, name: "", club: "", count: null },
            updateMessage: ""
        }
    },

    // ============================================================
    // LIFECYCLE: Load all members when page loads
    // Teacher File 2 + 3 pattern: async created()
    // ============================================================
    async created() {
        this.getMembers(baseUrl)
    },

    methods: {

        // ============================================================
        // SECTION 1: GET ALL
        // Teacher File 1 pattern: getAllCars() calls getCars(baseUrl)
        // ============================================================
        getAllMembers() {
            this.getMembers(baseUrl)
        },

        // ============================================================
        // SECTION 2: HELPER METHOD
        // Teacher File 1 pattern: getCars(url) is the async helper
        // Both getAllMembers + getByName use this helper
        // ============================================================
        async getMembers(url) {
            try {
                const response = await axios.get(url)
                this.members = response.data
                this.allMembers = this.members  // Teacher File 2: store all for client-side filter
                this.singleMember = null
            } catch (ex) {
                alert(ex.message)
            }
        },

        // ============================================================
        // SECTION 3: GET BY NAME - REST side filter
        // Teacher File 1 pattern: getByVendor(vendor) builds url + calls helper
        // ============================================================
        getByName(name) {
            const url = baseUrl + "/filter?minCount=0&nameFilter=" + encodeURIComponent(name)
            this.getMembers(url)
        },

        // ============================================================
        // SECTION 4: CLIENT-SIDE FILTER BY NAME
        // Teacher File 2 pattern: filterByTitle(title)
        // Uses allMembers to filter without calling API again
        // ============================================================
        filterByName(name) {
            this.members = this.allMembers.filter(m =>
                m.name.toLowerCase().includes(name.toLowerCase()))
        },

        // ============================================================
        // SECTION 5: SORT - client side
        // Teacher File 2 pattern: sortById, sortByTitle, sortByPrice
        // ============================================================
        sortById() {
            this.members.sort((m1, m2) => m1.id - m2.id)
        },
        sortByName() {
            this.members.sort((m1, m2) => m1.name.localeCompare(m2.name))
        },
        sortByCountAscending() {
            this.members.sort((m1, m2) => m1.count - m2.count)
        },
        sortByCountDescending() {
            this.members.sort((m1, m2) => m2.count - m1.count)
        },

        // ============================================================
        // SECTION 6: GET BY ID
        // Teacher File 1 pattern: getById(id)
        // ============================================================
        async getById(id) {
            if (id === null || id === undefined || isNaN(id) || id <= 0) {
                alert("Please enter a valid member ID")
                return
            }
            const url = baseUrl + "/" + id
            try {
                const response = await axios.get(url)
                this.singleMember = response.data
                this.members = []
            } catch (ex) {
                this.singleMember = null
                alert(ex.message)
            }
        },

        // ============================================================
        // SECTION 7: DELETE
        // Teacher File 1 pattern: deleteCar(deleteId)
        // ============================================================
        async deleteMember(deleteId) {
            if (deleteId === null || deleteId === undefined || isNaN(deleteId) || deleteId <= 0) {
                alert("Please enter a valid member ID")
                return
            }
            const url = baseUrl + "/" + deleteId
            try {
                const response = await axios.delete(url)
                this.deleteMessage = response.status + " " + response.statusText
                this.getAllMembers()
            } catch (ex) {
                alert(ex.message)
            }
        },

        // ============================================================
        // SECTION 8: ADD
        // Teacher File 1 pattern: addCar()
        // ============================================================
        async addMember() {
            if (this.addData.name === "" || this.addData.count === null ||
                this.addData.count < 100 || this.addData.count > 199) {
                alert("Please fill in all fields with valid values. Count must be 100-199")
                return
            }
            if (this.addData.name.trim().length < 2) {
                alert("Name must be at least 2 characters")
                return
            }
            try {
                const response = await axios.post(baseUrl, this.addData)
                this.addMessage = "response " + response.status + " " + response.statusText
                this.addData = { name: "", club: "", count: null }
                this.getAllMembers()
            } catch (ex) {
                alert(ex.message)
            }
        },

        // ============================================================
        // SECTION 9: UPDATE
        // Teacher File 1 pattern: updateCar()
        // ============================================================
        async updateMember() {
            if (this.updateData.id === null || this.updateData.id === undefined ||
                isNaN(this.updateData.id) || this.updateData.id <= 0) {
                alert("Please enter a valid member ID")
                return
            }
            if (this.updateData.name === "" || this.updateData.count === null ||
                this.updateData.count < 100 || this.updateData.count > 199) {
                alert("Please fill in all fields with valid values. Count must be 100-199")
                return
            }
            if (this.updateData.name.trim().length < 2) {
                alert("Name must be at least 2 characters")
                return
            }
            const url = baseUrl + "/" + this.updateData.id
            try {
                const response = await axios.put(url, this.updateData)
                this.updateMessage = "response " + response.status + " " + response.statusText
                this.updateData = { id: null, name: "", club: "", count: null }
                this.getAllMembers()
            } catch (ex) {
                alert(ex.message)
            }
        }
    }
}).mount("#app")