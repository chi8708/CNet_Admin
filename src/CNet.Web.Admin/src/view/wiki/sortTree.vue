<template>
    <div class="hello">
      <div class="core">
          <div class="abs-zone" v-if="editZoneDisplayBoolean">
              <div class="box">
                  <Input placeholder="Enter something..." style="width:200px" v-model="beforeSubmitNodeTitleString" />
                  <Button type="success" :style="{marginLeft:'5px'}" @click="submitNameEditFunc(1)">
                      <Icon type="md-checkmark" />
                  </Button>
                  <Button type="error" :style="{marginLeft:'5px'}" @click="submitNameEditFunc(0)">
                      <Icon type="md-close" />
                  </Button>
              </div>
          </div>
          <Tree :data="data5" :render="renderContent" show-checkbox multiple></Tree>
      </div>
    </div>
  </template><script>
  export default {
      data () {
          return {
              root:null,
              editZoneDisplayBoolean:false,
              beforeSubmitNodeTitleString:'',
              edit_root:null,
              edit_node:null,
              edit_data:null,
              data5: [
                  {
                      title: 'parent 1',
                      expand: true,
                      render: (h, { root, node, data }) => {
                          return h('span', {
                              style: {
                                  display: 'inline-block',
                                  width: '100%'
                              }
                          }, [
                              h('span', [
                                  h('Icon', {
                                      props: {
                                          type: 'ios-folder-outline'
                                      },
                                      style: {
                                          marginRight: '8px'
                                      }
                                  }),
                                  h('span', data.title)
                              ]),
                              h('span', {
                                  style: {
                                      display: 'inline-block',
                                      float: 'right',
                                      marginRight: '32px'
                                  }
                              }, [
                                  h('Button', {
                                      props: Object.assign({}, this.buttonProps, {
                                          icon: 'ios-add',
                                          type: 'primary'
                                      }),
                                      style: {
                                          width: '135px'
                                      },
                                      on: {
                                          click: () => { this.append(data) }
                                      }
                                  })
                              ])
                          ]);
                      },
                      children: [
                          {
                              title: 'child 1-1',
                              expand: true,
                              children: [
                                  {
                                      title: 'leaf 1-1-1',
                                      expand: true
                                  },
                                  {
                                      title: 'leaf 1-1-2',
                                      expand: true
                                  }
                              ]
                          },
                          {
                              title: 'child 1-2',
                              expand: true,
                              children: [
                                  {
                                      title: 'leaf 1-2-1',
                                      expand: true
                                  },
                                  {
                                      title: 'leaf 1-2-2',
                                      expand: true
                                  }
                              ]
                          }
                      ]
                  }
              ],
              buttonProps: {
                  type: 'default',
                  size: 'small',
              }
          }
      },
      methods: {
          renderContent (h, { root, node, data }) {
              return h('span', {
                  style: {
                      display: 'inline-block',
                      width: '100%'
                  }
              }, [
                  h('span', [
                      h('Icon', {
                          props: {
                              type: 'ios-paper-outline'
                          },
                          style: {
                              marginRight: '8px'
                          }
                      }),
                      h('span', data.title)
                  ]),
                  h('span', {
                      style: {
                          display: 'inline-block',
                          float: 'right',
                          marginRight: '32px'
                      }
                  }, [
                      h('Button', {
                          props: Object.assign({}, this.buttonProps, {
                              icon: 'ios-add'
                          }),
                          style: {
                              marginRight: '8px'
                          },
                          on: {
                              click: () => { this.append(data) }
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
                              click: () => { this.remove(root, node, data) }
                         }
                      }),
                      h('Button', {
                          props: Object.assign({}, this.buttonProps, {
                              icon: 'ios-create'
                          }),
                          style: {
                              marginRight: '8px'
                          },
                          on: {
                              click: () => { this.openEditName(root, node, data) }
                         }
                      }),
                      h('Button', {
                          props: Object.assign({}, this.buttonProps, {
                              icon: 'ios-arrow-round-up'
                          }),
                          on: {
                              click: () => { this.toUp(root, node, data) }
                         }
                      })
                  ])
              ]);
          },
          append (data) {
              const children = data.children || [];
              children.push({
                  title: 'appended node',
                  expand: true
              });
              this.$set(data, 'children', children);
          },
          remove (root, node, data) {
              const parentKey = root.find(el => el === node).parent;
              const parent = root.find(el => el.nodeKey === parentKey).node;
              const index = parent.children.indexOf(data);
              parent.children.splice(index, 1);
          },
          toUp (root, node, data) {
              const parentKey = root.find(el => el === node).parent;
              const parent = root.find(el => el.nodeKey === parentKey).node;
              const index = parent.children.indexOf(data);
              const children = parent.children
              children.unshift({
                  ...data
              });
              children.pop()
              this.$set(parent, 'children', children);
          },
          openEditName (root, node, data) {
              this.editZoneDisplayBoolean = true
              this.edit_root = root
              this.edit_node = node
              this.edit_data = data
              this.beforeSubmitNodeTitleString = this.edit_node.node.title
          },
          submitNameEditFunc(x){
              if (!x) {
                  this.editZoneDisplayBoolean = false
                  return
              } else {
                  this.edit_node.node.title = this.beforeSubmitNodeTitleString
                  this.editZoneDisplayBoolean = false
                  return
              }
          }
      }
  };
  </script><!-- Add "scoped" attribute to limit CSS to this component only -->