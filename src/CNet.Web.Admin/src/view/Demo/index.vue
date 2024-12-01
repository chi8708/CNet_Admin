<template>
    <div>
        <Row>
            <Col span="2">
                <Button type="primary" @click="wordView">word预览</Button>
            </Col>
            <Col span="2">
                <Button type="primary" @click="pdfView">pdf预览</Button>
            </Col>
        </Row>
       

    </div>
</template>

<script>
// import pdfview from './pdfview.vue';

export default {
    components: {
        //pdfview
    },
    name: '',
    data() {
        return {

        }
    },
    methods: {
        wordView() {
            this.openNewWindow("/demo/wordview/?filePath=http://localhost:8916/api/WikiMain/GetDocxFile?filePath=/FileUpload/wiki/1.docx");
        },
        pdfView() {
            this.openNewWindow("/demo/pdfview/?filePath=http://localhost:8916/api/WikiMain/GetPdfFile?filePath=/FileUpload/wiki/1.pdf");
        },
        openNewWindow(path) {
            const { href } = this.$router.resolve({
                //直接访问word
                path,
            });
            window.open(href, "_blank");//, "width=900, height=700"
        },
    }
}
</script>

<style lang="" scoped>
    
</style>