// use https (http secure).
// http (non-secure) will make the app complain about mixed content when running the app from Azure
const baseUrl = "https://localhost:7067/api/members"

Vue.createApp({
    data() {
        return {
            members: [],
            nameToGetBy: "",
            idToGetBy: null,
            singleMember: null,
            deleteId: null,
            deleteMessage: "",
            addData: { name: "", count: null },
            addMessage: "",
            updateData: { id: null, name: "", count: null },
            updateMessage: ""
        }
    },
 methods: {
    async getAllMembers() {
        await this.getMembers(baseUrl)
    },

    async getByName(name) {
        if (!name || name.trim() === "") {
            alert("Please enter a name to search")
            return
        }
        const url = baseUrl + "/filter?minCount=0&nameFilter=" + encodeURIComponent(name)
        try {
            const response = await axios.get(url)
            this.members = response.data
            this.singleMember = null
            if (this.members.length === 0) {
                alert(`No members found with name containing "${name}"`)
            }
        } catch (ex) {
            this.members = []
            alert(ex.message)
        }
    },

    async getMembers(url) {
        try {
            const response = await axios.get(url)
            this.members = response.data
            this.singleMember = null
        } catch (ex) {
            alert(ex.message)
        }
    },

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

    async deleteMember(deleteId) {
        if (deleteId === null || deleteId === undefined || isNaN(deleteId) || deleteId <= 0) {
            alert("Please enter a valid member ID")
            return
        }
        const url = baseUrl + "/" + deleteId
        try {
            const response = await axios.delete(url)
            this.deleteMessage = "Deleted successfully: " + response.status + " " + response.statusText
            await this.getAllMembers()
        } catch (ex) {
            alert(ex.message)
        }
    },

    async addMember() {
        if (this.addData.name === "" || this.addData.count === null || this.addData.count < 100 || this.addData.count > 199) {
            alert("Please fill in all fields with valid values. Count must be 100-199")
            return
        }
        if (this.addData.name.trim().length < 2) {
            alert("Name must be at least 2 characters")
            return
        }
        try {
            const response = await axios.post(baseUrl, this.addData)
            this.addMessage = "Added successfully: " + response.status + " " + response.statusText
            this.addData = { name: "", count: null }
            await this.getAllMembers()
        } catch (ex) {
            alert(ex.message)
        }
    },

    async updateMember() {
        if (this.updateData.id === null || this.updateData.id === undefined || isNaN(this.updateData.id) || this.updateData.id <= 0) {
            alert("Please enter a valid member ID")
            return
        }
        if (this.updateData.name === "" || this.updateData.count === null || this.updateData.count < 100 || this.updateData.count > 199) {
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
            this.updateMessage = "Updated successfully: " + response.status + " " + response.statusText
            this.updateData = { id: null, name: "", count: null }
            await this.getAllMembers()
        } catch (ex) {
            alert(ex.message)
        }
    }
}
}).mount("#app")