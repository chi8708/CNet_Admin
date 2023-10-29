<template>
  <div>
    <h3 v-if="isManage">
      分类
      <Button style="margin-left: 10px;"  class="search-btn" size="small"  icon="ios-add" @click="handleAdd">
        新增
     </Button>
    </h3>
    <div style="max-height:97%;overflow:auto;">
      <Tree :data="SortTree" @on-select-change="sortTreeChange" :render="renderContent" class="demo-tree-render"></Tree>
    </div>
    <Modal
      title="分类编辑"
      :mask-closable="false"
      v-model="modelEdit"
      width="500"
      scrollable
      footer-hide
      @on-ok="saveEdit"
    >
      <sort-edit ref="edit" v-if="modelEdit" :parent="this" :edit-row="eidtRow"></sort-edit>
    </Modal>
  </div>
</template>
<script>
import { getSorts } from "@/api/wikiSort";
import SortEdit  from './SortEdit.vue'
import {remove} from "@/api/wikiSort";
export default {
  name:'SortTree',
  components: {
    SortEdit
  },
  props: {
    parent: {},
    isManage:false,
  },
  data() {
    return {
      modelEdit:false,
      eidtRow: {},
      Sorts: [],
      SortTree: [],
      SortTreeItems: [],
      buttonProps: {
        type: 'default',
        size: 'small',
      }
    };
  },
  methods: {
    getSorts() {
      this.Sorts = [];
      this.SortTree = [];
      this.SortTreeItems = [];
      getSorts()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Sorts = data;
          this.loadSortTree();
        })
        .catch(err => { });
    },
    loadSortTree() {
      var treeRoot = this.Sorts.filter(p => p.parentCode == "");
      this.loadSortTreeChild(treeRoot);
    },
    loadSortTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.loadSortTreeItem(element);
        const child = this.Sorts.filter(p => p.parentCode == element.sortCode);
        if (child && child.length > 0) {
          this.loadSortTreeChild(child);
        }
      }
    },
    loadSortTreeItem(treeItemData) {
      var treeItem = {
        title: treeItemData.sortName,
        expand: true,
        value: treeItemData.sortCode,
      };
      this.SortTreeItems.push(treeItem);
      if (treeItemData.parentCode) {
        //foreach 无法终止循环
        this.SortTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(treeItem);
            return false;
          }
          return true;
        });

      }
      //跟节点
      else {
        this.SortTree.push(treeItem);
      }
    },
    sortSelect() {
      if (!this.Sorts || this.Sorts.length <= 0) {
        this.getSorts();
      }
    },
    sortTreeChange(data) {
      var item0 = data[0];
      if (!item0) {
        this.parent.modelSort = false;
        return;
      }
      this.parent.sortChange(item0.value, item0.title);
    },
    renderContent(h, { root, node, data }) {
      return h('span', [
        h('Icon', {
          props: {
            type: node.children ? 'ios-folder-outline' : 'ios-paper-outline'
          },
          style: {
            marginRight: '8px'
          }
        }),
        h('span', data.title),
        h('span', {
          style: {
            display: 'inline-block',
            float: 'right',
          }
        },!this.isManage?[]: [
          h('Button', {
            props: Object.assign({}, this.buttonProps, {
              icon: 'ios-create'
            }),
            style: {
              marginRight: '8px'
            },
            on: {
             // click: () => { this.append(data) }
             click: () => { this.handleEdit(data) }
            }
          }),
          h('Button', {
            props: Object.assign({}, this.buttonProps, {
              icon: 'ios-remove'
            }),
            style: {
              marginRight: '8px'
            },
            on: {
              click: () => { this.handleDelete(data) }
            }
          })
        ])
      ])
    },
    handleDelete(row) {
      const id = row.value;
      const rowData=this.Sorts.find(p=>p.sortCode==id);
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + rowData.sortName + "]?</p>",
        onOk: () => {
          this.remove(row);
        },
        onCancel: row => { }
      });
    },
    handleAdd() {
      this.modelEdit = true;
      this.isAdd = true;
      this.eidtRow = {};
    },
    handleEdit(row) {
      const id = row.value;
      const rowData=this.Sorts.find(p=>p.sortCode==id);
      this.modelEdit = true;
      this.isAdd = false;
      this.eidtRow = rowData;
    },
    remove(row) {
      var id = row.value;
      remove(id)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("删除成功");
            this.getSorts(this.selectedCode);
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => { });
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    },
  },
  mounted() {
    this.getSorts();
  }
};
</script>
<style scoped>
::v-deep .ivu-tree-title {
  width: 90%;
}
</style>